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

        <div class="form-group">
          <label
            for="patient-name"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-user text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Patient Name:
          </label>
          <input
            type="text"
            name="patient-name"
            id="patient-name"
            v-model="store.appointmentsForm.patientName"
            required
            placeholder="e.g., Juan Dela Cruz"
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 text-gray-800 placeholder-gray-400"
          />
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
import { ref, computed } from 'vue'

const store = useAppointmentStore()

const emit = defineEmits(['modalClose'])

const closeModal = () => {
  emit('modalClose')
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
</style>
