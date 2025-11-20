import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'

export const usePatientStore = defineStore('patientStore', () => {
  const searchterm = ref('')
  const patients = ref([])

  const fetchPatients = async () => {
    try {
      const response = await fetch('http://localhost:3000/patients')
      if (!response.ok) throw new Error('Failed to fetch patients')
      patients.value = await response.json()
      console.log('Patients fetched successfully')
    } catch (error) {
      console.error('Error fetching patients:', error)
    }
  }

  fetchPatients()

  const formPatient = ref({
    id: null,
    firstname: '',
    middlename: '',
    lastname: '',
    address: '',
    password: '',
    facebook: '',
    email: '',
    emergencyContact: ''.toString(),
  })

  const resetForm = () => {
    formPatient.value = {
      id: null,
      firstname: '',
      middlename: '',
      lastname: '',
      address: '',
      password: '',
      facebook: '',
      email: '',
      emergencyContact: '',
    }
  }

  const isEditMode = computed(() => !!formPatient.value.id)

  const setFormforEdit = (patient) => {
    formPatient.value = { ...patient }
  }

  const filteredpatients = computed(() => {
    const term = searchterm.value.toLowerCase()
    return patients.value.filter((patient) =>
      Object.values(patient).some((val) => String(val).toLowerCase().includes(term)),
    )
  })

  const addPatient = async (newPatient) => {
    try {
      const response = await fetch('http://localhost:3000/patients', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newPatient),
      })
      if (!response.ok) throw new Error('Failed to add patient')

      const addedPatient = await response.json()
      patients.value.push(addedPatient)
      console.log(
        `Patient ${addedPatient.firstname} ${addedPatient.middlename} ${addedPatient.lastname} added successfully`,
      )
      return true
    } catch (error) {
      console.error('Error adding patient:', error)
      return false
    }
  }

  const existingPatientDetails = (newPatient) => {
    const patientExist = patients.value.some(
      (p) =>
        p.firstname === newPatient.firstname &&
        p.lastname === newPatient.lastname &&
        p.middlename === newPatient.middlename,
    )
    if (patientExist) {
      console.error(
        `Patient ${newPatient.firstname} ${newPatient.middlename} ${newPatient.lastname} already exist`,
      )
      return false
    }
    return true
  }

  const deletePatient = async (id) => {
    try {
      const response = await fetch(`http://localhost:3000/patients/${id}`, {
        method: 'DELETE',
      })
      if (!response.ok) throw new Error('Failed to delete patient')
      patients.value = patients.value.filter((patient) => patient.id !== id)
      console.log(`Patient with ID ${id} has been deleted`)
      return true
    } catch (error) {
      console.error('Error deleting patient:', error)
      return false
    }
  }

  const editPatient = async (id, updatedPatient) => {
    try {
      const response = await fetch(`http://localhost:3000/patients/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedPatient),
      })
      if (!response.ok) throw new Error('Failed to update patient')

      const updatedData = await response.json()

      const index = patients.value.findIndex((patient) => patient.id === id)
      if (index !== -1) {
        patients.value[index] = updatedData
        console.log(`Patient with ID ${id} has been updated`)
        resetForm()
      }
    } catch (error) {
      console.error('Error updating patient:', error)
    }
  }

  const submitPatient = async () => {
    if (
      !emailVerification(formPatient.value) ||
      !phoneVerification(formPatient.value) ||
      !existingPatientDetails(formPatient.value)
    ) {
      return false
    }

    let success
    if (isEditMode.value) {
      success = await editPatient(formPatient.value.id, formPatient.value)
    } else {
      success = await addPatient(formPatient.value)
    }
    if (success) resetForm()
    return success
  }

  const emailVerification = (patient) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

    if (isEditMode.value) return true

    const emailExist = patients.value.some((p) => p.email === patient.email)

    if (patient.email && emailExist) {
      console.error(`Patient email ${patient.email} is already in use`)
      return false
    }
    if (patient.email && !emailRegex.test(patient.email)) {
      console.error(`Patient Email ${patient.email} is not a valid format`)
      return false
    }
    return true
  }

  const phoneVerification = (newPatient) => {
    const phoneNumber = String(newPatient.emergencyContact)
    if (phoneNumber.length !== 11) {
      console.error(
        `Patient phone number: ${newPatient.emergencyContact} should be 11 characters long`,
      )
      return false
    }
    return true
  }

  return {
    searchterm,
    patients,
    filteredpatients,
    formPatient,
    isEditMode,
    deletePatient,
    setFormforEdit,
    submitPatient,
    resetForm,
  }
})
