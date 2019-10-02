package datc.darius.controllers

import datc.darius.exceptions.NoSuchStudentException
import datc.darius.model.Student
import datc.darius.model.StudentRegistry
import org.springframework.web.bind.annotation.*
import java.rmi.NoSuchObjectException

@RestController
@RequestMapping("student")
class StudentController(val studentRegistry: StudentRegistry) {
    @GetMapping
    fun getAll() = studentRegistry.all

    private val studentNotFound = "Student not found"

    @GetMapping("{id}")
    fun getById(@PathVariable id: String) = studentRegistry.getByID(id) ?: throw NoSuchStudentException()

    @PostMapping
    fun add(@RequestBody student: Student) = studentRegistry.put(student)

    @PutMapping
    fun modify(@RequestBody student: Student) {
        val byID = studentRegistry.getByID(student.id) ?: throw NoSuchStudentException()
        val newStudent = Student(student.id,
                student.name ?: byID.name,
                student.faculty ?: byID.faculty,
                student.year ?: byID.year)
        studentRegistry.put(newStudent)
    }

    @DeleteMapping
    fun delete(@RequestParam id: String) = studentRegistry.remove(id)
}