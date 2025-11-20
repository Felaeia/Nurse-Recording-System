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
              {{
                patientRecordStore.isEditMode ? 'Update Medical Record' : 'Add New Medical Record'
              }}
            </h2>
            <p class="text-sm text-gray-500 mt-1">
              {{
                patientRecordStore.isEditMode
                  ? 'Edit the patient record details below'
                  : 'Fill in the patient record information'
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
        <div class="form-group">
          <label
            for="symptom"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-user-injured text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Symptom:
          </label>
          <textarea
            name="symptom"
            id="symptom"
            v-model="patientRecordStore.recordForm.symptom"
            rows="3"
            placeholder="Describe the patient's symptoms..."
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 resize-none text-gray-800 placeholder-gray-400"
            required          
            ></textarea>
        </div>

        <div class="form-group">
          <label
            for="diagnosis"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-stethoscope text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Diagnosis:
          </label>
          <textarea
            name="diagnosis"
            id="diagnosis"
            v-model="patientRecordStore.recordForm.diagnosis"
            rows="3"
            placeholder="Enter the diagnosis..."
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 resize-none text-gray-800 placeholder-gray-400"
          ></textarea>
        </div>

        <div class="form-group">
          <label
            for="notes"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-notes-medical text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Notes:
          </label>
          <textarea
            name="notes"
            id="notes"
            v-model="patientRecordStore.recordForm.notes"
            rows="4"
            placeholder="Add any additional notes..."
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 resize-none text-gray-800 placeholder-gray-400"
          ></textarea>
        </div>

        <div class="form-group">
          <label
            for="treatment"
            class="block text-sm font-semibold text-gray-700 mb-2 flex items-center gap-2"
          >
            <span
              class="w-6 h-6 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center"
            >
              <i
                class="fa-solid fa-pills text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              ></i>
            </span>
            Possible Treatment:
          </label>
          <textarea
            name="treatment"
            id="treatment"
            v-model="patientRecordStore.recordForm.treatment"
            rows="3"
            placeholder="Describe the treatment plan..."
            class="w-full px-4 py-3 rounded-xl border border-gray-200 focus:outline-none focus:ring-2 focus:ring-[#2933FF]/50 focus:border-transparent transition-all duration-300 resize-none text-gray-800 placeholder-gray-400"
          ></textarea>
        </div>

        <div class="flex justify-between gap-3 pt-4 border-t border-gray-100">
          <button
            v-if="patientRecordStore.isEditMode"
            @click="handlePrintRecord"
            type="button"
            class="px-6 py-3 bg-gradient-to-r from-purple-500 to-purple-600 text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95"
          >
            <i class="fa-solid fa-print mr-1"></i>
            Print Record
          </button>
          
          <div class="flex gap-3 ml-auto">
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
              {{ patientRecordStore.isEditMode ? 'Update Record' : 'Add Record' }}
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router'
import { usePatientRecord } from '@/stores/patientRecord'

const patientRecordStore = usePatientRecord()
const route = useRoute()
const router = useRouter()

const emit = defineEmits(['modalClose'])

const closeModal = () => {
  emit('modalClose')
}

const submitHandler = async () => {
  patientRecordStore.recordForm.patientId = String(route.params.id)

  // Only set date if adding a new record (not in edit mode)
  if (!patientRecordStore.isEditMode) {
    patientRecordStore.recordForm.date = new Date().toISOString().split('T')[0]
  }

  const success = await patientRecordStore.submitRecord()

  if (success) {
    closeModal()
  }
}

const handlePrintRecord = () => {
  const patientId = route.params.id
  const recordId = patientRecordStore.recordForm.id
  
  // Navigate to print view with both patient and record IDs
  router.push({
    name: 'printview',
    params: {
      patientId: patientId,
      recordId: recordId
    }
  })
  
  closeModal()
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
</style>