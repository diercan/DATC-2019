package datc.darius.lab2

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication
import org.springframework.context.annotation.ComponentScan

@ComponentScan("datc.darius")
@SpringBootApplication
class Lab2Application

fun main(args: Array<String>) {
    runApplication<Lab2Application>(*args)
}
