import com.google.api.client.extensions.java6.auth.oauth2.AuthorizationCodeInstalledApp
import com.google.api.client.extensions.jetty.auth.oauth2.LocalServerReceiver
import com.google.api.client.googleapis.auth.oauth2.GoogleAuthorizationCodeFlow
import com.google.api.client.googleapis.auth.oauth2.GoogleClientSecrets
import com.google.api.client.googleapis.javanet.GoogleNetHttpTransport
import com.google.api.client.http.HttpRequestInitializer
import com.google.api.client.http.InputStreamContent
import com.google.api.client.http.javanet.NetHttpTransport
import com.google.api.client.json.jackson2.JacksonFactory
import com.google.api.services.drive.Drive
import com.google.api.services.drive.DriveScopes
import com.google.api.services.drive.model.File
import org.mortbay.jetty.MimeTypes
import java.nio.file.Path
import java.nio.file.Paths.get

fun listFilesWithGoogleServices() {
    val service = getService()
    val fileList = service.files().list().setPageSize(10)
            .setFields("nextPageToken, files(id, name)")
            .execute()

    fileList.files.forEach { println("${it.name}: ${it.id}") }
}

private fun getService(): Drive {
    val trustedTransport = GoogleNetHttpTransport.newTrustedTransport()

    return Drive.Builder(trustedTransport, jacksonFactory(), getCredentials(trustedTransport))
            .setApplicationName("DATC-drive").build()
}

fun getCredentials(trustedTransport: NetHttpTransport?): HttpRequestInitializer? {
    val clientSecrets = GoogleClientSecrets.load(jacksonFactory(), getClientIdFilePath().toFile().reader())

    val flow = GoogleAuthorizationCodeFlow.Builder(trustedTransport, jacksonFactory(), clientSecrets, scopes()).build()
    val receiver = LocalServerReceiver.Builder()
            .setPort(8888)
            .build()

    return AuthorizationCodeInstalledApp(flow, receiver).authorize("user")
}

fun scopes(): List<String> = listOf(DriveScopes.DRIVE_METADATA_READONLY, DriveScopes.DRIVE_FILE)

private fun jacksonFactory() = JacksonFactory.getDefaultInstance()

fun getClientIdFilePath(): Path = get(System.getProperty("user.home"), "Documents", "client_id.json")


fun uploadFile() {
    val service = getService()
    val file = get(System.getProperty("user.home"), "Documents", "dummy file datc.txt").toFile()
    val driveFile = File()
    driveFile.name = file.name
    driveFile.mimeType = MimeTypes.TEXT_PLAIN
    service.files().create(driveFile, InputStreamContent(null, file.inputStream()))
            .execute().let { print(it) }
}