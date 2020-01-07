import com.microsoft.azure.storage.table.TableServiceEntity

class Student(partitionKey: String?,
              rowKey: String?) : TableServiceEntity(partitionKey, rowKey) {
    constructor(partitionKey: String?,
                rowKey: String?, name: String?, email: String?, year: Int?) : this(partitionKey, rowKey) {
        this.email = email
        this.name = name
        this.year = year
    }

    constructor() : this(null, null)

    var name: String? = null
    var email: String? = null
    var year: Int? = null

    override fun toString(): String {
        return "$partitionKey, $rowKey, $name, $email, $year"
    }
}