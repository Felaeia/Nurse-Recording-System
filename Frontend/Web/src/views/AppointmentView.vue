<template>
  <div
    class="min-h-screen bg-gradient-to-br from-gray-50 via-blue-50/30 to-red-50/20 font-poppins p-10"
  >
    <div class="max-w-7xl mx-auto">
      <div class="header-section bg-white rounded-2xl shadow-lg p-8 mb-8 border border-gray-100">
        <div class="flex items-center justify-between">
          <div class="flex items-center gap-4">
            <div
              class="w-16 h-16 rounded-full bg-gradient-to-r from-[#2933FF] to-[#FF5451] flex items-center justify-center text-white shadow-lg"
            >
              <i class="fa-solid fa-calendar-check text-2xl"></i>
            </div>
            <div>
              <h2
                class="text-3xl font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              >
                Appointments
              </h2>
              <p class="text-gray-500 text-sm mt-1">Manage patient appointments</p>
            </div>
          </div>
          <button
            @click="handleNewAppointment"
            class="px-6 py-3 bg-gradient-to-r from-[#2933FF] to-[#FF5451] text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95 flex items-center gap-2"
          >
            <i class="fa-solid fa-plus"></i>
            New Appointment
          </button>
        </div>
      </div>

      <div class="search mb-8 relative">
        <div class="absolute left-5 top-1/2 -translate-y-1/2 pointer-events-none">
          <i
            class="fa-solid fa-magnifying-glass text-xl bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
          ></i>
        </div>
        <input
          type="text"
          v-model="searchQuery"
          placeholder="Search appointments..."
          class="w-full pl-14 pr-6 py-4 rounded-2xl shadow-lg focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:shadow-xl bg-white text-gray-800 placeholder-gray-400 transition-all duration-300 border border-gray-100"
        />
      </div>

      <div v-if="filteredAppointments.length === 0" class="text-center py-12">
        <div
          class="w-20 h-20 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center mx-auto mb-4"
        >
          <i
            class="fa-solid fa-calendar-xmark text-3xl bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
          ></i>
        </div>
        <p class="text-gray-500 text-lg">No appointments found</p>
      </div>

      <div class="appointment-list grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <div
          v-for="appointment in filteredAppointments"
          :key="appointment.id"
          class="bg-white rounded-2xl p-6 shadow-lg hover:shadow-2xl transition-all hover:-translate-y-2 duration-300 border border-gray-100 group relative overflow-hidden"
        >
          <div
            class="absolute inset-0 bg-gradient-to-br from-[#2933FF]/5 to-[#FF5451]/5 opacity-0 group-hover:opacity-100 transition-opacity duration-300"
          ></div>

          <div class="relative z-10">
            <div class="flex items-start justify-between mb-4">
              <div class="flex items-center gap-2">
                <span
                  class="w-10 h-10 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
                >
                  <i
                    class="fa-solid fa-calendar-days text-sm bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  ></i>
                </span>
                <div>
                  <h2
                    class="text-xl font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  >
                    {{ appointment.reason }}
                  </h2>
                  <p class="text-xs text-gray-400 mt-0.5">
                    {{ appointment.appointmentId || `#${appointment.id}` }}
                  </p>
                </div>
              </div>
              <div class="flex gap-2">
                <button
                  @click="handleEdit(appointment)"
                  class="w-8 h-8 rounded-lg bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center hover:shadow-md transition-all hover:scale-110 active:scale-95"
                  title="Edit Appointment"
                >
                  <i
                    class="fa-solid fa-pen-to-square text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  ></i>
                </button>
                <button
                  @click="confirmDelete(appointment)"
                  class="w-8 h-8 rounded-lg bg-red-50 flex items-center justify-center hover:shadow-md transition-all hover:scale-110 active:scale-95"
                  title="Delete Appointment"
                >
                  <i class="fa-solid fa-trash text-xs text-red-500"></i>
                </button>
              </div>
            </div>

            <div class="space-y-3">
              <div class="flex items-start gap-2">
                <span
                  class="w-8 h-8 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center flex-shrink-0 mt-1"
                >
                  <i
                    class="fa-solid fa-user text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  ></i>
                </span>
                <div>
                  <h3 class="text-sm font-semibold text-gray-700 mb-1">Patient</h3>
                  <p class="text-sm text-gray-600">{{ getPatientName(appointment.patientId) }}</p>
                </div>
              </div>

              <div class="flex items-start gap-2">
                <span
                  class="w-8 h-8 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center flex-shrink-0 mt-1"
                >
                  <i
                    class="fa-solid fa-calendar text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  ></i>
                </span>
                <div>
                  <h3 class="text-sm font-semibold text-gray-700 mb-1">Date</h3>
                  <p class="text-sm text-gray-600">{{ formatDate(appointment.date) }}</p>
                </div>
              </div>

              <div class="flex items-start gap-2">
                <span
                  class="w-8 h-8 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center flex-shrink-0 mt-1"
                >
                  <i
                    class="fa-solid fa-clock text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  ></i>
                </span>
                <div>
                  <h3 class="text-sm font-semibold text-gray-700 mb-1">Time</h3>
                  <p class="text-sm text-gray-600">{{ formatTime(appointment.time) }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Appointment Handler Modal -->
    <AppointmentHandler v-if="showAppointmentModal" @modalClose="closeAppointmentModal" />

    <!-- Delete Confirmation Modal -->
    <div
      v-if="showDeleteModal"
      class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 font-poppins"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full border border-gray-100 p-6">
        <div class="flex items-center gap-3 mb-4">
          <div class="w-12 h-12 rounded-full bg-red-50 flex items-center justify-center">
            <i class="fa-solid fa-exclamation-triangle text-xl text-red-500"></i>
          </div>
          <div>
            <h3 class="text-xl font-bold text-gray-800">Delete Appointment</h3>
            <p class="text-sm text-gray-500">This action cannot be undone</p>
          </div>
        </div>
        <p class="text-gray-600 mb-6">
          Are you sure you want to delete the appointment for
          <span class="font-semibold">{{ appointmentToDelete?.reason }}</span
          >?
        </p>
        <div class="flex justify-end gap-3">
          <button
            @click="cancelDelete"
            class="px-6 py-3 bg-gray-100 text-gray-700 text-sm font-semibold rounded-xl transition-all hover:bg-gray-200 hover:shadow-md active:scale-95"
          >
            Cancel
          </button>
          <button
            @click="handleDelete"
            class="px-6 py-3 bg-gradient-to-r from-red-500 to-red-600 text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useAppointmentStore } from '@/stores/AppointmentStore'
import { usePatientStore } from '@/stores/patientsStore'
import AppointmentHandler from '@/modals/AppoitmentHandler.vue'
import { ref, computed } from 'vue'

const store = useAppointmentStore()
const patientStore = usePatientStore()

const searchQuery = ref('')
const showAppointmentModal = ref(false)
const showDeleteModal = ref(false)
const appointmentToDelete = ref(null)

const filteredAppointments = computed(() => {
  if (!searchQuery.value) return store.appointments

  const query = searchQuery.value.toLowerCase()
  return store.appointments.filter(
    (appointment) =>
      appointment.reason?.toLowerCase().includes(query) ||
      appointment.appointmentId?.toLowerCase().includes(query) ||
      getPatientName(appointment.patientId).toLowerCase().includes(query) ||
      appointment.date?.includes(query),
  )
})

const getPatientName = (patientId) => {
  const patient = patientStore.patients.find((p) => p.id === patientId)
  if (patient) {
    return `${patient.firstname} ${patient.lastname}`
  }
  return 'Unknown Patient'
}

const formatDate = (dateString) => {
  if (!dateString) return 'N/A'
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
}

const formatTime = (timeString) => {
  if (!timeString) return 'N/A'
  const [hours, minutes] = timeString.split(':')
  const hour = parseInt(hours)
  const ampm = hour >= 12 ? 'PM' : 'AM'
  const displayHour = hour % 12 || 12
  return `${displayHour}:${minutes} ${ampm}`
}

const handleNewAppointment = () => {
  store.resetForm()
  showAppointmentModal.value = true
}

const handleEdit = (appointment) => {
  store.setFormforEdit(appointment)
  showAppointmentModal.value = true
}

const closeAppointmentModal = () => {
  showAppointmentModal.value = false
}

const confirmDelete = (appointment) => {
  appointmentToDelete.value = appointment
  showDeleteModal.value = true
}

const cancelDelete = () => {
  appointmentToDelete.value = null
  showDeleteModal.value = false
}

const handleDelete = async () => {
  if (appointmentToDelete.value) {
    await store.deleteAppointment(appointmentToDelete.value.id)
    showDeleteModal.value = false
    appointmentToDelete.value = null
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
</style>
