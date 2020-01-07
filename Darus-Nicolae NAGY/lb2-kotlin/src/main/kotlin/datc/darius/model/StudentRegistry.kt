package datc.darius.model

import org.springframework.stereotype.Component

@Component
class StudentRegistry : AbstractRegistry<Student, String>() {
    override fun getID(entity: Student) = entity.id
}