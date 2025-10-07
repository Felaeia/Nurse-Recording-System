import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'

export const usePatientStore = defineStore('patientStore', () => {
  const searchterm = ref('')

  const storedPatients = localStorage.getItem('patients')
  const patients = ref(
    storedPatients
      ? JSON.parse(storedPatients)
      : [
          {
            id: 1,
            firstname: 'Arcgel',
            middlename: 'Bataluna',
            lastname: 'Chavez',
            address: 'Tugas Gun-ob Lapu Lapu City',
            password: 'chavezpassword',
            facebook: 'facebook.com/arcgel',
            email: 'arcgel@email.com',
            emergencyContact: '09112233445',
          },
          {
            id: 2,
            firstname: 'Rajie Mae',
            middlename: '',
            lastname: ' Villa',
            address: 'Ibabao Lapu Lapu City',
            password: 'villapassword',
            facebook: '',
            email: 'rajie@email.com',
            emergencyContact: '09223344556',
          },
          {
            id: 3,
            firstname: 'Raizza Mae',
            middlename: '',
            lastname: 'Empeno',
            address: 'Kagudoy Lapu Lapu City',
            password: 'empenopassword',
            facebook: '',
            email: '',
            emergencyContact: '09334455667',
          },
        ],
  )

  const formPatient = ref({
    id: null,
    firstname: '',
    middlename: '',
    lastname: '',
    password: '',
    facebook: '',
    email: '',
    emergencyContact: '',
  })

  const resetForm = () => {
    formPatient.value = {
      id: null,
      firstname: '',
      middlename: '',
      lastname: '',
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

  watch(
    patients,
    (newPatients) => {
      localStorage.setItem('patients', JSON.stringify(newPatients))
    },
    { deep: true },
  )

  const filteredpatients = computed(() => {
    const term = searchterm.value.toLowerCase()
    return patients.value.filter((patient) =>
      Object.values(patient).some((val) => String(val).toLowerCase().includes(term)),
    )
  })

  const addPatient = (patient) => {
    if (!patient.id) {
      patient.id = Date.now()
    }
    patients.value.push(patient)
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

  const deletePatient = (patientId) => {
    patients.value = patients.value.filter((patient) => patient.id !== patientId)
    console.log(`Patient with ID ${patientId} has been deleted`)
  }

  const editPatient = (patientId, updatedPatient) => {
    const index = patients.value.findIndex((patient) => patient.id === patientId)
    if (index !== -1) {
      patients.value[index] = { ...updatedPatient, id: patientId }
      console.log(`patient with ID ${patientId} has been updated`)
    } else {
      console.error(`Patient with ID ${patientId} not found`)
    }
  }

  const submitPatient = () => {
    if (isEditMode.value) {
      editPatient(formPatient.value.id, formPatient.value)
    } else {
      const isPatientValid = patientVerification(formPatient.value)
      if (isPatientValid) {
        addPatient(formPatient.value)
      } else {
        return false
      }
    }
    resetForm()
    return true
  }

  const emailVerification = (newPatient) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    const emailExist = patients.value.some((p) => p.email === newPatient.email)
    if (emailExist) {
      console.error(`Patient ${newPatient.email} is already in used`)
      return false
    }
    if (!emailRegex.test(newPatient.email)) {
      console.error(`Patient Email ${newPatient.email} is not a valid format`)
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

  const patientVerification = (newPatient) => {
    return (
      existingPatientDetails(newPatient) &&
      emailVerification(newPatient) &&
      phoneVerification(newPatient)
    )
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
