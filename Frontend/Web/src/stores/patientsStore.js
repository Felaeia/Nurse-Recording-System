import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'

export const usePatientStore = defineStore('patientStore', () => {
  const searchterm = ref('')
  const patients = ref([])

  const fetchPatients = async () => {
    try {
      const response = await fetch('https://localhost:7031/api/NurseUser/view/all_users', {
        method: 'GET',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' }
      })
      if (!response.ok) throw new Error('Failed to fetch patients')
      patients.value = await response.json()
      console.log('Patients fetched successfully')
    } catch (error) {
      console.error('Error fetching patients:', error)
    }
  }

  fetchPatients()

  const formPatient = ref({
    userId: null,
    userName: '',
    email: '',
    role: '',
    firstName: '',
    middleName: '',
    lastName: '',
    contactNumber: '',
    address: '',
    password: '',
  })

  const resetForm = () => {
    formPatient.value = {
      userId: null,
      userName: '',
      email: '',
      role: '',
      firstName: '',
      middleName: '',
      lastName: '',
      contactNumber: '',
      address: '',
      password: '',
    }
  }

  const isEditMode = computed(() => !!formPatient.value.userId)

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
      const payload = {
        userName: newPatient.firstName + newPatient.lastName,
        password: newPatient.password,
        email: newPatient.email,
        firstName: newPatient.firstName,
        middleName: newPatient.middleName,
        address: null,
        lastName: newPatient.lastName,
        contactNumber: newPatient.contactNumber,
      }
      const response = await fetch('https://localhost:7031/api/CreateUser/user', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })
      if (!response.ok) throw new Error('Failed to add patient')

      await fetchPatients()
      console.log(
        `Patient ${newPatient.firstName} ${newPatient.middleName} ${newPatient.lastName} added successfully`,
      )
      return true
    } catch (error) {
      console.error('Error adding patient:', error)
      return false
    }
  }

  const existingPatientDetails = (newPatient) => {
    if (isEditMode.value) return true
    const patientExist = patients.value.some(
      (p) =>
        p.firstName === newPatient.firstName &&
        p.lastName === newPatient.lastName &&
        p.middleName === newPatient.middleName,
    )
    if (patientExist) {
      console.error(
        `Patient ${newPatient.firstName} ${newPatient.middleName} ${newPatient.lastName} already exist`,
      )
      return false
    }
    return true
  }

  const deletePatient = async (id) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'))
      const nurseId = nurseData?.nurseDetails?.nurseId

      if (!nurseId) {
        throw new Error('Nurse details not found. Please log in again.')
      }

      const response = await fetch(`https://localhost:7031/api/AdminUsers/delete/user/${id}`, {
        method: 'DELETE',
        credentials: 'include',
        headers: {
          'X-Deleted-By': nurseId,
        },
      })
      if (!response.ok) throw new Error('Failed to delete patient')
      await fetchPatients()
      console.log(`Patient with ID ${id} has been deleted`)
      return true
    } catch (error) {
      console.error('Error deleting patient:', error)
      return false
    }
  }

  const editPatient = async (updatedPatient) => {
    try {
      // TODO: Replace with the correct API endpoint for updating a patient
      // and construct the correct payload
      const response = await fetch(`http://localhost:3000/patients/${updatedPatient.userId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedPatient),
      })
      if (!response.ok) throw new Error('Failed to update patient')

      await fetchPatients()
      console.log(`Patient with ID ${updatedPatient.userId} has been updated`)
      resetForm()
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
      success = await editPatient(formPatient.value)
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
    const phoneNumber = String(newPatient.contactNumber)
    if (phoneNumber.length < 10) {
      console.error(
        `Patient phone number: ${newPatient.contactNumber} should be at least 10 characters long`,
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
    addPatient,
    editPatient,
  }
})

