import { defineStore } from 'pinia'
import { ref, watch, computed } from 'vue'

export const usePatientRecord = defineStore('patientRecord', () => {
  const storedRecords = localStorage.getItem('patientRecords')
  const patientRecords = ref(
    storedRecords
      ? JSON.parse(storedRecords)
      : [
          {
            recordId: 'r001',
            patientId: '1',
            date: '2025-08-01',
            diagnosis: 'Hypertension',
            symptom: 'Headaches, dizziness', // Added symptom
            treatment: 'Medication prescribed',
            notes: 'Follow-up in 2 weeks',
          },
          {
            recordId: 'r002',
            patientId: '1',
            date: '2025-08-01',
            diagnosis: 'Diarhhea',
            symptom: 'Stomach cramps, dehydration', // Added symptom
            treatment: 'Medication prescribed',
            notes: 'Follow-up Tommorow',
          },
          {
            recordId: 'r003',
            patientId: '3',
            date: '2025-08-01',
            diagnosis: 'Common Cold',
            symptom: 'Cough, sneezing, sore throat', // Added symptom
            treatment: 'Medication prescribed, and lots of rest',
            notes: 'Follow-up in 2 days',
          },
        ],
  )

  watch(
    patientRecords,
    (newValue) => {
      localStorage.setItem('patientRecords', JSON.stringify(newValue))
    },
    { deep: true },
  )

  const recordForm = ref({
    recordId: null,
    patientId: null,
    date: '',
    diagnosis: '',
    symptom: '',
    treatment: '',
    notes: '',
  })

  const resetRecordForm = () => {
    recordForm.value = {
      recordId: null,
      patientId: null,
      date: '',
      diagnosis: '',
      symptom: '',
      treatment: '',
      notes: '',
    }
  }
  const isEditMode = computed(() => !!recordForm.value.recordId)

  const setFormforEdit = (record) => {
    recordForm.value = { ...record }
  }

  const getpatient = (id) => {
    return patientRecords.value.filter((r) => r.patientId === String(id))
  }

  const addRecords = (newRecord) => {
    newRecord.recordId = 'r' + (patientRecords.value.length + 1).toString().padStart(3, '0')
    patientRecords.value.push(newRecord)
  }

  const deleteRecord = (recordId) => {
    patientRecords.value = patientRecords.value.filter((record) => record.recordId !== recordId)
    console.log(`Record with ${recordId} has been deleted`)
  }

  const editRecord = (recordId, updatedRecord) => {
    const index = patientRecords.value.findIndex((record) => record.recordId === recordId)
    if (index !== -1) {
      patientRecords.value[index] = { ...updatedRecord, recordId: recordId }
      console.log(`Record with ID ${recordId} has been updated`)
    } else {
      console.error(`Record with ID ${recordId} not found`)
    }
  }

  const submitRecord = () => {
    if (isEditMode.value) {
      editRecord(recordForm.value.recordId, recordForm.value)
    } else {
      addRecords(recordForm.value)
    }
    resetRecordForm()
    return true
  }

  return {
    patientRecords,
    getpatient,
    recordForm,
    isEditMode,
    setFormforEdit,
    submitRecord,
    deleteRecord,
    resetRecordForm,
  }
})
