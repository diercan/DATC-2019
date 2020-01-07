import com.microsoft.azure.storage.CloudStorageAccount
import com.microsoft.azure.storage.StorageCredentials
import com.microsoft.azure.storage.table.CloudTable

const val tableName = "NagyTable"

fun main() {
    val account = CloudStorageAccount(StorageCredentials.tryParseCredentials("DefaultEndpointsProtocol=https;AccountName=datcdemoluni;AccountKey=Xf0DheNxYHU8BAyOV0snfLt3Y8R9kO7TADAvhLHi31f8LQdU04q3BTGrUKQVIV6BzvwHPYEPSfd4aElDhHxYhQ==;EndpointSuffix=core.windows.net"), true)
    val table = getTableReference(account)
    val studentsService = StudentsService(table)

    val student = Student("UPT-AC", "19609093245876")
    student.name = "Nebunu Paul"
    studentsService.updateStudent(student)

    studentsService.all.forEach { println(it) }
}

private fun getTableReference(account: CloudStorageAccount): CloudTable {
    val tableClient = account.createCloudTableClient()
    val table = tableClient.getTableReference(tableName)
    table.createIfNotExists()
    return table
}