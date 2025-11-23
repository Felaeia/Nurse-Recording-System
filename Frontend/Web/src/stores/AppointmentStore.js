import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAppointmentStore = defineStore('appointmentStore', () => {
  const appointments = ref([])
  const loading = ref(false)
  const error = ref(null)

  const appointmentsForm = ref({
    id: null,
    appointmentId: null,
    date: '',
    time: '',
    reason: '',
    patientName: '',
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
      date: '',
      time: '',
      reason: '',
      patientName: '',
    }
  }

  const isEditMode = computed(() => !!appointmentsForm.value.id)

  const fetchAppointments = async () => {
    try {
      const response = await fetch('https://localhost:7031/api/NurseAppointmentSchedule/view_appointment_list', {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json'
        },
      })

      if (response.status === 401) {
        console.error("Session expired")
      }

      const data = await response.json()
      appointments.value = data
    } catch (err) {
      console.error(err)
    }
  }

  const addAppointment = async (appointmentData) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'));
      const nurseId = nurseData?.nurseDetails?.nurseId;
      const createdBy = nurseData?.userName;
      const patientName = appointmentData.patientName.trim();

      if (!nurseId || !createdBy) {
        throw new Error('Nurse details not found. Please log in again.');
      }

      const appointmentTime = new Date(`${appointmentData.date}T${appointmentData.time}`).toISOString();

      const payload = {
        appointmentTime: appointmentTime,
        patientName: patientName,
        appointmentDescription: appointmentData.reason,
        nurseId: nurseId,
        createdBy: createdBy
      };

      const response = await fetch('https://localhost:7031/api/NurseAppointmentSchedule/create_appointment', {
        method: 'POST',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to add appointment: ${errorText}`);
      }

      await fetchAppointments();

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

    if (!form.date || !form.time || !form.reason || !form.patientName) {
      console.error('Date, Time, Reason, and Patient Name are required.')
      return false
    }

    if (isEditMode.value) {
      return await editAppointment(form.id, form)
    } else {
      return await addAppointment(form)
    }
  }

  const setFormforEdit = (appointment) => {
    appointmentsForm.value = { ...appointment }
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
    isEditMode,
    loading,
    error,
    fetchAppointments,
    submitAppointment,
    setFormforEdit,
    deleteAppointment,
    resetForm,
    editAppointment,
  }
})