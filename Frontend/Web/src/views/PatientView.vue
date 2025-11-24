
<template>
  <div class="min-h-screen flex font-poppins bg-gray-50 text-gray-900">
    <div class="sidebar w-96 p-8 bg-white shadow-lg flex flex-col overflow-auto">
      <div class="flex flex-col items-center mb-10 pb-8 border-b border-gray-200">
        <div class="gradient-logo w-28 h-28 mb-5"></div>
        <h1
          class="text-4xl font-extrabold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent text-center"
        >
          ACLC CLINIC
        </h1>
        <p class="text-sm text-gray-500 mt-2">Patient Management</p>
      </div>
      <PatientHandler />
    </div>

    <div class="main flex-1 p-10 overflow-auto">
      <div class="flex items-center justify-between mb-10 gap-6">
        <div class="search flex-1 relative max-w-3xl">
          <div class="absolute left-5 top-1/2 -translate-y-1/2 pointer-events-none">
            <i class="fa-solid fa-magnifying-glass text-gray-400 text-lg"></i>
          </div>
          <input
            type="text"
            v-model="store.searchterm"
            placeholder="Search patient by name..."
            class="w-full pl-14 pr-6 py-4 rounded-full border-2 border-gray-200 text-gray-900 transition outline-none focus:border-[#2933FF] focus:ring-2 focus:ring-[#2933FF]/20"
          />
        </div>

        <button
          @click="appointments()"
          class="px-8 py-4 bg-gradient-to-r from-[#2933FF] to-[#FF5451] text-white rounded-full shadow-md hover:shadow-xl transition-all font-semibold whitespace-nowrap"
        >
          <i class="fa-solid fa-calendar-days mr-2"></i>Appointments
        </button>

        <button
          @click="logout()"
          class="px-8 py-4 bg-gradient-to-r from-red-500 to-red-600 text-white rounded-full shadow-md hover:shadow-xl transition-all font-semibold whitespace-nowrap"
        >
          <i class="fa-solid fa-right-from-bracket mr-2"></i>Logout
        </button>
      </div>

      <div class="content grid gap-6 grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-3">
        <div
          v-for="patient in store.filteredpatients"
          :key="patient.userId"
          @click="patientsrecord(patient.userId)"
          class="cursor-pointer bg-white rounded-2xl p-6 shadow-sm hover:shadow-xl transition-all relative overflow-hidden border border-gray-100 hover:border-[#2933FF]/30 flex flex-col"
        >
          <div
            class="absolute inset-0 bg-gradient-to-br from-[#2933FF]/5 to-[#FF5451]/5 opacity-0 hover:opacity-100 transition-opacity rounded-2xl"
          ></div>

          <div class="relative z-10 flex flex-col h-full">
            <div class="mb-4 pb-4 border-b border-gray-100">
              <div class="flex items-center gap-3">
                <span class="w-5 h-5 flex items-center justify-center flex-shrink-0 mt-0.5">
                  <i class="fa-solid fa-user text-[#2933FF]"></i>
                </span>
                <h3
                  class="text-lg font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent leading-tight break-words"
                >
                  {{ patient.firstName }} {{ patient.middleName ? patient.middleName + ' ' : ''
                  }}{{ patient.lastName }}
                </h3>
              </div>
              <p class="text-xs text-gray-400 mt-1 ml-8">
                {{ patient.userName }} (#{{ patient.userId }})
              </p>
            </div>

            <div class="space-y-3 text-gray-600 text-sm mb-5 flex-1">
              <div class="flex items-start gap-3">
                <span class="w-5 h-5 flex items-center justify-center flex-shrink-0 mt-0.5">
                  <i class="fa-solid fa-phone text-[#2933FF]"></i>
                </span>
                <span class="break-words leading-relaxed">{{ patient.contactNumber }}</span>
              </div>
              <div class="flex items-start gap-3">
                <span class="w-5 h-5 flex items-center justify-center flex-shrink-0 mt-0.5">
                  <i class="fa-solid fa-envelope text-[#FF5451]"></i>
                </span>
                <span class="break-words leading-relaxed">{{ patient.email }}</span>
              </div>
            </div>

            <div class="flex justify-end gap-3 mt-auto">
              <button
                @click.stop="handleEdit(patient)"
                class="px-4 py-2 text-blue-600 border border-blue-600 rounded-lg text-sm font-semibold hover:bg-blue-600 hover:text-white transition-all"
              >
                <i class="fa-solid fa-pen mr-1"></i>Edit
              </button>
              <button
                @click.stop="confirmDelete(patient)"
                class="px-4 py-2 text-red-600 border border-red-600 rounded-lg text-sm font-semibold hover:bg-red-600 hover:text-white transition-all"
              >
                <i class="fa-solid fa-trash mr-1"></i>Delete
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <EditPatientHandlerModal v-if="showEditModal" @close="showEditModal = false" />

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
            <h3 class="text-xl font-bold text-gray-800">Delete Patient</h3>
            <p class="text-sm text-gray-500">This action cannot be undone</p>
          </div>
        </div>
        <p class="text-gray-600 mb-6">
          Are you sure you want to delete patient
          <span class="font-semibold">{{ patientToDelete?.firstName }} {{ patientToDelete?.lastName }}</span
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
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { usePatientStore } from '@/stores/patientsStore'
import { useAuthStore } from '@/stores/authStore'
import PatientHandler from '@/modals/PatientHandler.vue'
import EditPatientHandlerModal from '@/modals/EditPatientHandlerModal.vue'

const store = usePatientStore()
const auth = useAuthStore()
const router = useRouter()
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const patientToDelete = ref(null)

const patientsrecord = (id) => {
  router.push({ name: 'patientrecords', params: { id } })
}

const appointments = (id) => {
  router.push({ name: 'appointments' })
}

const handleEdit = (patient) => {
  store.setFormforEdit(patient)
  showEditModal.value = true
}

const confirmDelete = (patient) => {
  patientToDelete.value = patient
  showDeleteModal.value = true
}

const cancelDelete = () => {
  patientToDelete.value = null
  showDeleteModal.value = false
}

const handleDelete = async () => {
  if (patientToDelete.value) {
    await store.deletePatient(patientToDelete.value.userId)
    showDeleteModal.value = false
    patientToDelete.value = null
  }
}

const logout = () => {
  auth.logout()
  router.push({ name: 'login' })
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

.gradient-logo {
  mask: url('../../images/ACLC.svg') center / contain no-repeat;
  -webkit-mask: url('../../images/ACLC.svg') center / contain no-repeat;
  background: linear-gradient(135deg, #2933ff, #ff5451);
  background-size: 200% 200%;
  width: 7rem;
  height: 7rem;
  display: inline-block;
}
</style>
