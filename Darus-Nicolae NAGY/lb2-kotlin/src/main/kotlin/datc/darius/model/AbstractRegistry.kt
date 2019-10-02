package datc.darius.model

import java.util.*

abstract class AbstractRegistry<TYPE, ID> {
    private val map: MutableMap<ID, TYPE> = HashMap()

    val all: Collection<TYPE>
        get() = map.values

    val allIDs: Set<ID>
        get() = map.keys

    open fun getByID(id: ID): TYPE? {
        return map[id]
    }

    fun put(entity: TYPE, id: ID): TYPE? {
        return map.put(id, entity)
    }

    fun put(entity: TYPE): TYPE? {
        return map.put(getID(entity), entity)
    }

    fun remove(id: ID): TYPE? {
        return map.remove(id)
    }

    protected abstract fun getID(entity: TYPE): ID
}
