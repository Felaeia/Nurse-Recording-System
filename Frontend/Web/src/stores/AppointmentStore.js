import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'
import { usePatientStore } from './patientsStore'

export const useAppointmentStore = defineStore('appointmentStore', () => {
  const patientStore = usePatientStore()
  const appointments = ref([])
  const loading = ref(false)
  const error = ref(null)

  const patientSearchTerm = ref('')
  const selectedPatientId = ref(null)

  const appointmentsForm = ref({
    id: null,
    appointmentId: null,
    patientId: null,
    date: '',
    time: '',
    reason: '',
  })

  // --- ID Generation Logic ---
  const generateAppointmentId = () => {
    if (appointments.value.length === 0) {
      return 'P-001'
    }

    // Find the appointment with the highest sequential number
    const lastAppointment = appointments.value.reduce(
      (latest, current) => {
        if (!latest.appointmentId || !current.appointmentId) return latest

        const latestNum = parseInt(latest.appointmentId.split('-')[1])
        const currentNum = parseInt(current.appointmentId.split('-')[1])

        return currentNum > latestNum ? current : latest
      },
      { appointmentId: 'P-000' },
    )

    const lastId = lastAppointment.appointmentId
    const numberPart = parseInt(lastId.split('-')[1])
    const nextNumber = numberPart + 1

    // Pad the number with leading zeros
    const nextId = 'P-' + String(nextNumber).padStart(3, '0')
    return nextId
  }
  // ----------------------------

  const resetForm = () => {
    appointmentsForm.value = {
      id: null,
      appointmentId: null,
      patientId: null,
      date: '',
      time: '',
      reason: '',
    }
    selectedPatientId.value = null
    patientSearchTerm.value = ''
  }

  const isEditMode = computed(() => !!appointmentsForm.value.id)

  // Filter patients based on search term for dynamic dropdown
  const filteredPatients = computed(() => {
    if (!patientSearchTerm.value || patientSearchTerm.value.trim() === '') {
      return []
    }

    const term = patientSearchTerm.value.toLowerCase().trim()

    return patientStore.patients.filter((patient) => {
      try {
        const firstname = (patient.firstname || '').toLowerCase()
        const lastname = (patient.lastname || '').toLowerCase()
        const middlename = (patient.middlename || '').toLowerCase()
        const contact = String(patient.emergencyContact || '')
        const fullName = `${firstname} ${middlename} ${lastname}`.toLowerCase()

        return (
          fullName.includes(term) ||
          firstname.includes(term) ||
          lastname.includes(term) ||
          middlename.includes(term) ||
          contact.includes(term)
        )
      } catch (error) {
        console.error('Error filtering patient:', patient, error)
        return false
      }
    })
  })

  // Computed property to get the details of the selected registered patient
  const selectedPatient = computed(() => {
    if (selectedPatientId.value) {
      const patient = patientStore.patients.find((p) => p.id === selectedPatientId.value)

      if (patient) {
        appointmentsForm.value.patientId = patient.id
      }
      return patient
    }
    appointmentsForm.value.patientId = null
    return null
  })

  // Function to be called when a search result is clicked
  const selectPatient = (patient) => {
    selectedPatientId.value = patient.id
    // Clear the search term to close the dropdown
    patientSearchTerm.value = ''
  }

  // Debug watcher
  watch(patientSearchTerm, (newVal) => {
    console.log('Search term changed:', newVal)
  })

  watch(selectedPatientId, (newVal) => {
    console.log('Selected patient ID:', newVal)
  })

  const fetchAppointments = async () => {
  try {
    const response = await fetch('https://localhost:7031/api/NurseAppointmentSchedule/view_appointment_list', {
      method: 'GET',
      credentials: 'include', 
      headers: {
        'Content-Type': 'application/json' 
        // No 'Authorization' header needed anymore!
      },
    })

    if (response.status === 401) {
       // If we get a 401, it means the Cookie is missing or expired
       console.error("Session expired")
       // Optional: Redirect to login
    }

    const data = await response.json()
    appointments.value = data
  } catch (err) {
    console.error(err)
  }
}

  // fetchAppointments()

  const addAppointment = async (appointmentData) => {
    try {
      const newAppointmentId = generateAppointmentId()

      const payload = {
        ...appointmentData,
        appointmentId: newAppointmentId,
      }

      const response = await fetch('http://localhost:3000/appointments', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })

      if (!response.ok) throw new Error('Failed to add appointment')
      const newAppointment = await response.json()
      appointments.value.push(newAppointment)
      resetForm()
      return true
    } catch (error) {
      console.error('Error adding appointment:', error)
      return false
    }
  }

  const editAppointment = async (id, updatedAppointment) => {
    try {
      const response = await fetch(`http://localhost:3000/appointments/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedAppointment),
      })
      if (!response.ok) throw new Error('Failed to update appointment')

      const updatedData = await response.json()

      const index = appointments.value.findIndex((a) => a.id === id)
      if (index !== -1) {
        appointments.value[index] = updatedData
        resetForm()
      }
      return true
    } catch (error) {
      console.error('Error updating appointment:', error)
      return false
    }
  }

  const submitAppointment = async () => {
    const form = appointmentsForm.value

    if (!form.date || !form.time || !form.reason || !form.patientId) {
      console.error('Date, Time, Reason, and a Registered Patient are required.')
      return false
    }

    if (isEditMode.value) {
      form.patientId = selectedPatientId.value
      return await editAppointment(form.id, form)
    } else {
      return await addAppointment(form)
    }
  }

  const setFormforEdit = (appointment) => {
    appointmentsForm.value = { ...appointment }
    selectedPatientId.value = appointment.patientId

    // Clear the search term when editing (no need to show it)
    patientSearchTerm.value = ''
  }

  const deleteAppointment = async (id) => {
    try {
      const response = await fetch(`http://localhost:3000/appointments/${id}`, {
        method: 'DELETE',
      })
      if (!response.ok) throw new Error('Failed to delete appointment')
      appointments.value = appointments.value.filter((a) => a.id !== id)
      return true
    } catch (error) {
      console.error('Error deleting appointment:', error)
      return false
    }
  }

  

  return {
    appointments,
    appointmentsForm,
    selectedPatientId,
    selectedPatient,
    patientSearchTerm,
    filteredPatients,
    isEditMode,
    loading,
    error,
    selectPatient,
    fetchAppointments,
    submitAppointment,
    setFormforEdit,
    deleteAppointment,
    resetForm,
    editAppointment,
  }
})
