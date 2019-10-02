package datc.darius.handlers

import datc.darius.exceptions.NoSuchStudentException
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.ControllerAdvice
import org.springframework.web.bind.annotation.ExceptionHandler
import org.springframework.web.context.request.WebRequest

@ControllerAdvice
class DefaultExceptionHandler {

    @ExceptionHandler(NoSuchStudentException::class)
    fun handleNoSuchStudentException(e: NoSuchStudentException, request: WebRequest): ResponseEntity<String> {
        return ResponseEntity(e.message, HttpStatus.NOT_FOUND)
    }
}