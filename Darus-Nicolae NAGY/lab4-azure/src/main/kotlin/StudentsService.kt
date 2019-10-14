import com.microsoft.azure.storage.table.CloudTable
import com.microsoft.azure.storage.table.TableOperation
import com.microsoft.azure.storage.table.TableQuery
import com.microsoft.azure.storage.table.TableResult

class StudentsService(val table: CloudTable) {
    val all get() = table.execute(TableQuery.from(Student::class.java))

    fun addStudent(student: Student) {
        table.execute(TableOperation.insert(student))
    }

    fun deleteStudent(student: Student): TableResult? {
        student.etag = "*"
        return table.execute(TableOperation.delete(student))
    }

    fun updateStudent(student: Student): TableResult? {
        student.etag = "*"
        return table.execute(TableOperation.merge(student))
    }
}