import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { useUserFormStore } from './userFormStore'
import { usePatientStore } from './patientsStore'
import { useAuthStore } from './authStore'

export const usePrintStore = defineStore('printStore', () => {
  const userFormStore = useUserFormStore()
  const patientStore = usePatientStore()
  const authStore = useAuthStore()

  const selectedPatientId = ref(null)
  const selectedFormId = ref(null)

  // Get patient details
  const patient = computed(() => {
    if (!selectedPatientId.value) return null
    return patientStore.patients.find((p) => p.userId === selectedPatientId.value)
  })

  // Get specific form or all forms for patient
  const forms = computed(() => {
    if (!selectedPatientId.value) return []

    const allForms = userFormStore.forms

    if (selectedFormId.value) {
      return allForms.filter((f) => f.formId === selectedFormId.value)
    }

    return allForms
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
    const { firstName, middleName, lastName } = patient.value
    return `${firstName} ${middleName ? middleName + ' ' : ''}${lastName}`
  })

  // Set data for printing
  const setPrintData = (patientId, formId = null) => {
    selectedPatientId.value = patientId
    selectedFormId.value = formId
    userFormStore.fetchUserForms(patientId)
  }

  // Reset print data
  const resetPrintData = () => {
    selectedPatientId.value = null
    selectedFormId.value = null
  }

  // Trigger browser print dialog
  const printDocument = () => {
    window.print()
  }

  return {
    selectedPatientId,
    selectedFormId,
    patient,
    forms,
    nurse,
    todaysDate,
    patientFullName,
    setPrintData,
    resetPrintData,
    printDocument,
  }
})
