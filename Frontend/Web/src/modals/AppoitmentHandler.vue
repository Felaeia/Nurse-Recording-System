<template>
  <div
    class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 font-poppins"
    @click.self="closeModal"
  >
    <div
      class="bg-white rounded-2xl shadow-2xl max-w-2xl w-full max-h-[90vh] overflow-auto border border-gray-100"
    >
      <div class="sticky top-0 bg-white border-b border-gray-100 p-6 rounded-t-2xl z-10">
        <div class="flex items-center justify-between">
          <div>
            <h2
              class="text-2xl font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
            >
              {{ store.isEditMode ? 'Edit Appointment' : 'New Appointment' }}
            </h2>
            <p class="text-sm text-gray-500 mt-1">
              {{
                store.isEditMode
                  ? 'Update appointment details below'
                  : 'Schedule a new patient appointment'
              }}
            </p>
          </div>
          <button
            @click="closeModal"
            class="w-10 h-10 rounded-full bg-gray-100 flex items-center justify-center hover:bg-gray-200 transition-all hover:scale-110 active:scale-95"
            title="Close"
          >
            <i class="fa-solid fa-xmark text-gray-600"></i>
          </button>
        </div>
      </div>

      <form @submit.prevent="submitHandler" class="p-6 space-y-6">
        <div
          v-if="store.isEditMode"
          class="bg-gradient-to-r from-[#2933FF]/5 to-[#FF5451]/5 rounded-xl p-4 border border-gray-200"
        >
          <label
            for="appointment-id"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-hashtag text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Appointment ID:
          </label>
          <input
            type="text"
            id="appointment-id"
            :value="store.appointmentsForm.appointmentId"
            readonly
            class="w-full px-4 py-3 rounded-xl border border-gray-200 bg-gray-50 text-gray-600 font-mono text-sm"
          />
        </div>
        <div
          v-else
          class="bg-gradient-to-r from-[#2933FF]/5 to-[#FF5451]/5 rounded-xl p-4 border border-gray-200"
        >
          <div class="flex items-center gap-2">
            <i class="fa-solid fa-info-circle text-[#2933FF]"></i>
            <p class="text-sm text-gray-700">
              New Appointment ID will be generated automatically (e.g., P-001).
            </p>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="form-group">
            <label
              for="date"
              class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
            >
              <span
                class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
              >
                <i
                  class="fa-solid fa-calendar text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                ></i>
              </span>
              Date:
            </label>
            <input
              type="date"
              name="date"
              id="date"
              v-model="store.appointmentsForm.date"
              required
              class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 text-gray-800"
            />
          </div>

          <div class="form-group">
            <label
              for="time"
              class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
            >
              <span
                class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
              >
                <i
                  class="fa-solid fa-clock text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                ></i>
              </span>
              Time:
            </label>
            <input
              type="time"
              name="time"
              id="time"
              v-model="store.appointmentsForm.time"
              required
              class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 text-gray-800"
            />
          </div>
        </div>

        <div class="form-group">
          <label
            for="reason"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-stethoscope text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Reason for Visit:
          </label>
          <input
            type="text"
            name="reason"
            id="reason"
            v-model="store.appointmentsForm.reason"
            required
            placeholder="e.g., Annual checkup, Follow-up consultation..."
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 text-gray-800 placeholder-gray-400"
          />
        </div>

        <div class="form-group patient-search-container">
          <label
            for="patient-search"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-user-magnifying-glass text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Search & Select Patient:
          </label>
          <div class="relative">
            <input
              type="text"
              id="patient-search"
              v-model="store.patientSearchTerm"
              placeholder="Start typing patient name or contact..."
              :disabled="store.isEditMode"
              class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 text-gray-800 placeholder-gray-400 disabled:bg-gray-50 disabled:text-gray-500"
              @focus="handleSearchFocus"
            />

            <ul v-if="showDropdown && store.filteredPatients.length >= 0" class="patient-results">
              <li
                v-for="patient in store.filteredPatients"
                :key="patient.id"
                @click="handlePatientSelect(patient)"
                class="flex items-center gap-3 p-3 hover:bg-gradient-to-r hover:from-[#2933FF]/5 hover:to-[#FF5451]/5 cursor-pointer transition-all border-b border-gray-100 last:border-b-0"
              >
                <div
                  class="w-10 h-10 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center flex-shrink-0"
                >
                  <span
                    class="text-sm font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                  >
                    {{ patient.firstname?.[0] || '' }}{{ patient.lastname?.[0] || '' }}
                  </span>
                </div>
                <div>
                  <p class="text-sm font-semibold text-gray-800">
                    {{ patient.firstname }} {{ patient.middlename }} {{ patient.lastname }}
                  </p>
                  <p class="text-xs text-gray-500">{{ patient.emergencyContact }}</p>
                </div>
              </li>

              <li
                v-if="store.patientSearchTerm && store.filteredPatients.length === 0"
                class="p-4 text-center text-gray-500 text-sm"
              >
                No patients found matching "{{ store.patientSearchTerm }}"
              </li>
            </ul>
          </div>
        </div>

        <div
          v-if="store.selectedPatient"
          class="bg-gradient-to-r from-[#2933FF]/5 to-[#FF5451]/5 rounded-xl p-5 border border-gray-200"
        >
          <div class="flex items-center gap-3 mb-3">
            <div
              class="w-12 h-12 rounded-full bg-gradient-to-r from-[#2933FF] to-[#FF5451] flex items-center justify-center text-white font-bold shadow-lg"
            >
              {{ store.selectedPatient.firstname?.[0] || ''
              }}{{ store.selectedPatient.lastname?.[0] || '' }}
            </div>
            <div>
              <h3
                class="text-lg font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              >
                Selected Patient
              </h3>
              <p class="text-xs text-gray-500">ID: {{ store.selectedPatient.id }}</p>
            </div>
          </div>
          <div class="space-y-2 text-sm">
            <div class="flex items-center gap-2">
              <i class="fa-solid fa-user text-[#2933FF] w-4"></i>
              <span class="text-gray-700"
                ><strong>Name:</strong> {{ store.selectedPatient.firstname }}
                {{ store.selectedPatient.middlename }} {{ store.selectedPatient.lastname }}</span
              >
            </div>
            <div class="flex items-center gap-2">
              <i class="fa-solid fa-phone text-[#2933FF] w-4"></i>
              <span class="text-gray-700"
                ><strong>Contact:</strong> {{ store.selectedPatient.emergencyContact }}</span
              >
            </div>
            <div class="flex items-center gap-2">
              <i class="fa-solid fa-envelope text-[#2933FF] w-4"></i>
              <span class="text-gray-700"
                ><strong>Email:</strong> {{ store.selectedPatient.email }}</span
              >
            </div>
          </div>
        </div>

        <div class="flex justify-end gap-3 pt-4 border-t border-gray-100">
          <button
            @click="closeModal"
            type="button"
            class="px-6 py-3 bg-gray-100 text-gray-700 text-sm font-semibold rounded-xl transition-all hover:bg-gray-200 hover:shadow-md active:scale-95"
          >
            <i class="fa-solid fa-xmark mr-1"></i>
            Cancel
          </button>
          <button
            type="submit"
            class="px-6 py-3 bg-gradient-to-r from-[#2933FF] to-[#FF5451] text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95"
          >
            <i class="fa-solid fa-check mr-1"></i>
            {{ store.isEditMode ? 'Update Appointment' : 'Schedule Appointment' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { useAppointmentStore } from '@/stores/AppointmentStore'
import { usePatientStore } from '@/stores/patientsStore'
import { ref, computed } from 'vue'

const store = useAppointmentStore()
const patientStore = usePatientStore()

const emit = defineEmits(['modalClose'])

const showDropdown = computed(() => {
  return !store.isEditMode && store.patientSearchTerm && store.patientSearchTerm.length > 0
})

const closeModal = () => {
  emit('modalClose')
}

const handleSearchFocus = () => {
  // Allow re-searching even if a patient is selected
  if (!store.isEditMode && store.selectedPatient) {
    store.patientSearchTerm = ''
  }
}

const handlePatientSelect = (patient) => {
  store.selectPatient(patient)
}

const submitHandler = async () => {
  const success = await store.submitAppointment()

  if (success) {
    closeModal()
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

.patient-search-container {
  position: relative;
}

.patient-results {
  list-style: none;
  padding: 0;
  margin: 0;
  margin-top: 8px;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  position: absolute;
  z-index: 10;
  background: white;
  width: 100%;
  max-height: 300px;
  overflow-y: auto;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
}

.patient-results::-webkit-scrollbar {
  width: 8px;
}

.patient-results::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

.patient-results::-webkit-scrollbar-thumb {
  background: linear-gradient(to bottom, #2933ff, #ff5451);
  border-radius: 10px;
}
</style>
