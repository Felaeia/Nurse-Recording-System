import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

const API_BASE_URL = 'http://localhost:3000'

export const usePatientRecord = defineStore('patientRecord', () => {
  const patientRecords = ref([])

  // --- API Fetching ---
  const fetchRecords = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/patientRecords`)
      if (!response.ok) throw new Error('Failed to fetch patient records')
      const data = await response.json()
      // Use the server's primary key 'id' consistently.
      patientRecords.value = data
      console.log('Patient records data fetched from API.')
    } catch (error) {
      console.error('Error fetching patient records:', error)
    }
  }

  fetchRecords()

  // Re-introduced 'recordId' as the human-readable ID
  const recordForm = ref({
    id: null, // Server's auto-increment ID (internal)
    patientId: '',
    recordId: '', // Human-readable generated ID (e.g., R-001)
    date: '',
    diagnosis: '',
    symptom: '',
    treatment: '',
    notes: '',
  })

  const resetRecordForm = () => {
    recordForm.value = {
      id: null,
      patientId: '',
      recordId: '',
      date: '',
      diagnosis: '',
      symptom: '',
      treatment: '',
      notes: '',
    }
  }

  // Check against 'id' property
  const isEditMode = computed(() => !!recordForm.value.id)

  const setFormforEdit = (record) => {
    // Record fetched from API now must have both 'id' and 'recordId'
    recordForm.value = { ...record }
  }

  // This correctly filters records by the associated patientId
  const getpatient = (id) => {
    return patientRecords.value.filter((r) => r.patientId === String(id))
  }

  // Helper to generate the next sequential ID (R-001, R-002, etc.)
  const getNextRecordId = () => {
    const records = patientRecords.value
    if (records.length === 0) {
      return 'R-001'
    }

    // Find the highest number used in existing recordIds
    const maxNum = records.reduce((max, record) => {
      if (
        record.recordId &&
        typeof record.recordId === 'string' &&
        record.recordId.startsWith('R-')
      ) {
        const num = parseInt(record.recordId.substring(2), 10)
        return num > max ? num : max
      }
      return max
    }, 0)

    const nextNum = maxNum + 1
    // Format as R-padded number
    return 'R-' + nextNum.toString().padStart(3, '0')
  }

  // --- CRUD: Add Record ---
  const addRecord = async (newRecord) => {
    const recordToPost = { ...newRecord }

    // 1. Generate the human-readable ID if it's a new record
    if (!recordToPost.recordId) {
      recordToPost.recordId = getNextRecordId()
    }

    // 2. Remove the 'id' field to allow the server (json-server) to auto-generate it.
    // This is crucial for POST requests.
    delete recordToPost.id

    try {
      const response = await fetch(`${API_BASE_URL}/patientRecords`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(recordToPost),
      })

      // Check the response status explicitly for a non-success code
      if (!response.ok) {
        const errorText = await response.text()
        console.error('Server response error:', errorText)
        throw new Error('Failed to add record')
      }

      const addedRecord = await response.json()

      // Add the server-generated record (which now correctly has both 'id' and 'recordId')
      patientRecords.value.push(addedRecord)

      console.log(
        `Record ${addedRecord.recordId} for patient ${newRecord.patientId} added successfully.`,
      )
      return true
    } catch (error) {
      console.error('Error adding record:', error)
      return false
    }
  }

  // --- CRUD: Edit Record ---
  const editRecord = async (id, updatedRecord) => {
    const serverId = id // The id passed is the server's primary key

    const recordToPut = { ...updatedRecord }
    // Remove the 'id' property from the request body for safety
    delete recordToPut.id

    try {
      // Use the server ID for the endpoint
      const response = await fetch(`${API_BASE_URL}/patientRecords/${serverId}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(recordToPut),
      })
      if (!response.ok) throw new Error('Failed to update record')

      const updatedData = await response.json()

      // Find and update the local array by 'id'
      const index = patientRecords.value.findIndex((record) => record.id === id)
      if (index !== -1) {
        patientRecords.value[index] = updatedData
        console.log(`Record with ID ${id} has been updated.`)
      }
      return true
    } catch (error) {
      console.error('Error updating record:', error)
      return false
    }
  }

  // --- CRUD: Delete Record ---
  const deleteRecord = async (id) => {
    const serverId = id // The id passed is the server's primary key

    try {
      const response = await fetch(`${API_BASE_URL}/patientRecords/${serverId}`, {
        method: 'DELETE',
      })
      if (!response.ok) throw new Error('Failed to delete record')

      // Filter based on the server 'id'
      patientRecords.value = patientRecords.value.filter((record) => record.id !== id)
      console.log(`Record with ${id} has been deleted.`)
      return true
    } catch (error) {
      console.error('Error deleting record:', error)
      return false
    }
  }

  const submitRecord = async () => {
    let success = false
    if (isEditMode.value) {
      // Use form's 'id' property (server ID)
      success = await editRecord(recordForm.value.id, recordForm.value)
    } else {
      success = await addRecord(recordForm.value)
    }
    if (success) {
      resetRecordForm()
    }
    return success
  }

  // Updated return object to reflect the use of 'id' in the records
  return {
    patientRecords,
    recordForm,
    isEditMode,
    resetRecordForm,
    setFormforEdit,
    getpatient,
    deleteRecord,
    submitRecord,
    fetchRecords,
  }
})
