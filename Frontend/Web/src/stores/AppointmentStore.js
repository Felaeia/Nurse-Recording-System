import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAppointmentStore = defineStore('appointmentStore', () => {
  const appointments = ref([])
  const loading = ref(false)
  const error = ref(null)

  const appointmentsForm = ref({
    appointmentId: null,
    date: '',
    time: '',
    appointmentDescription: '',
    patientName: '',
    nurseId: null,
    updatedBy: ''
  })

  const resetForm = () => {
    appointmentsForm.value = {
      appointmentId: null,
      date: '',
      time: '',
      appointmentDescription: '',
      patientName: '',
      nurseId: null,
      updatedBy: ''
    }
  }

  const isEditMode = computed(() => !!appointmentsForm.value.appointmentId)

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
        appointmentDescription: appointmentData.appointmentDescription,
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

  const editAppointment = async (updatedAppointment) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'))
      const nurseId = nurseData?.nurseDetails?.nurseId
      const updatedBy = nurseData?.userName

      if (!nurseId || !updatedBy) {
        throw new Error('Nurse details not found. Please log in again.')
      }

      const appointmentTime = new Date(`${updatedAppointment.date}T${updatedAppointment.time}`).toISOString()

      const payload = {
        appointmentTime: appointmentTime,
        patientName: updatedAppointment.patientName.trim(),
        appointmentDescription: updatedAppointment.appointmentDescription,
        nurseId: nurseId,
        updatedBy: updatedBy
      }

      const response = await fetch(`https://localhost:7031/api/NurseAppointmentSchedule/update_appointment/${updatedAppointment.appointmentId}`, {
        method: 'PUT',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })

      if (!response.ok) {
        const errorText = await response.text()
        throw new Error(`Failed to update appointment: ${errorText}`)
      }

      await fetchAppointments()
      resetForm()
      return true
    } catch (error) {
      console.error('Error updating appointment:', error)
      return false
    }
  }

  const submitAppointment = async () => {
    const form = appointmentsForm.value

    if (!form.date || !form.time || !form.appointmentDescription || !form.patientName) {
      console.error('Date, Time, Reason, and Patient Name are required.')
      return false
    }

    if (isEditMode.value) {
      return await editAppointment(form)
    } else {
      return await addAppointment(form)
    }
  }

  const setFormforEdit = (appointment) => {
    const appointmentDate = new Date(appointment.appointmentTime);
    const date = appointmentDate.toISOString().split('T')[0];
    const time = appointmentDate.toTimeString().split(' ')[0].substring(0, 5);
  
    appointmentsForm.value = { 
      ...appointment,
      date: date,
      time: time
    }
  }

  const deleteAppointment = async (id) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'))
      const nurseId = nurseData?.nurseDetails?.nurseId

      if (!nurseId) {
        throw new Error('Nurse details not found. Please log in again.')
      }

      const payload = {
        deletedByNurseId: nurseId
      }

      const response = await fetch(`https://localhost:7031/api/NurseAppointmentSchedule/delete_appointment/${id}`, {
        method: 'DELETE',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })
      
      if (!response.ok) throw new Error('Failed to delete appointment')
      
      await fetchAppointments()
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