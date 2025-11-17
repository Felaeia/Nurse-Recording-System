import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { usePatientRecord } from './patientRecord'
import { usePatientStore } from './patientsStore'
import { useAuthStore } from './authStore'

export const usePrintStore = defineStore('printStore', () => {
  const patientRecordStore = usePatientRecord()
  const patientStore = usePatientStore()
  const authStore = useAuthStore()

  const selectedPatientId = ref(null)
  const selectedRecordId = ref(null)

  // Get patient details
  const patient = computed(() => {
    if (!selectedPatientId.value) return null
    return patientStore.patients.find((p) => p.id === selectedPatientId.value)
  })

  // Get specific record or all records for patient
  const records = computed(() => {
    if (!selectedPatientId.value) return []

    const allRecords = patientRecordStore.getpatient(selectedPatientId.value)

    if (selectedRecordId.value) {
      return allRecords.filter((r) => r.id === selectedRecordId.value)
    }

    return allRecords
  })

  // Get current nurse information
  const nurse = computed(() => authStore.nurse)

  // Format today's date
  const todaysDate = computed(() => {
    const date = new Date()
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    })
  })

  // Format patient full name
  const patientFullName = computed(() => {
    if (!patient.value) return 'Unknown Patient'
    const { firstname, middlename, lastname } = patient.value
    return `${firstname} ${middlename ? middlename + ' ' : ''}${lastname}`
  })

  // Set data for printing
  const setPrintData = (patientId, recordId = null) => {
    selectedPatientId.value = patientId
    selectedRecordId.value = recordId
  }

  // Reset print data
  const resetPrintData = () => {
    selectedPatientId.value = null
    selectedRecordId.value = null
  }

  // Trigger browser print dialog
  const printDocument = () => {
    window.print()
  }

  return {
    selectedPatientId,
    selectedRecordId,
    patient,
    records,
    nurse,
    todaysDate,
    patientFullName,
    setPrintData,
    resetPrintData,
    printDocument,
  }
})
