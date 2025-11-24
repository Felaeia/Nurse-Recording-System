<template>
  <div class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 font-poppins">
    <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full border border-gray-100 p-6">
      <div class="flex items-center justify-between mb-6">
        <h3 class="text-xl font-bold text-gray-800">Edit User Form</h3>
        <button @click="$emit('close')" class="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center hover:bg-gray-200 transition-all">
          <i class="fa-solid fa-xmark text-gray-600"></i>
        </button>
      </div>
      <form @submit.prevent="submitHandler" class="space-y-4">
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
            Issue Type:
          </label>
        <input
          type="text"
          v-model="editableForm.issueType"
          placeholder="Issue Type"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />
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
            Issue Description:
          </label>
        <input
          type="text"
          v-model="editableForm.issueDescryption"
          placeholder="Issue Description"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />
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
            Status:
          </label>
        <input
          type="text"
          v-model="editableForm.status"
          placeholder="Status"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />
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
          v-model="editableForm.patientName"
          placeholder="Patient Name"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />
        <div class="flex gap-3 mt-4">
          <button
            type="submit"
            class="flex-1 px-4 py-2 rounded-[5px] border-2 border-blue-600 text-blue-600 font-semibold hover:bg-blue-600 hover:text-white transition"
          >
            Update Form
          </button>
          <button
            type="button"
            @click="$emit('close')"
            class="flex-1 px-4 py-2 rounded-[5px] border-2 border-gray-400 text-gray-600 font-semibold hover:bg-gray-400 hover:text-white transition"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useUserFormStore } from '@/stores/userFormStore'

const props = defineProps({
  form: {
    type: Object,
    required: true,
  },
  userId: {
    type: Number,
    required: true,
  }
})

const emit = defineEmits(['close'])
const store = useUserFormStore()

const editableForm = ref({ ...props.form })

watch(() => props.form, (newForm) => {
  editableForm.value = { ...newForm }
})

const submitHandler = async () => {
  await store.updateUserForm(editableForm.value, props.userId)
  emit('close')
}
</script>