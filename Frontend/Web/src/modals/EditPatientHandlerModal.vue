<template>
  <div class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 font-poppins">
    <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full border border-gray-100 p-6">
      <div class="flex items-center justify-between mb-6">
        <h3 class="text-xl font-bold text-gray-800">Edit Patient</h3>
        <button @click="$emit('close')" class="w-8 h-8 rounded-full bg-gray-100 flex items-center justify-center hover:bg-gray-200 transition-all">
          <i class="fa-solid fa-xmark text-gray-600"></i>
        </button>
      </div>
      <form @submit.prevent="submitHandler" class="space-y-4">
        <input
          type="text"
          v-model="store.formPatient.firstName"
          placeholder="Firstname"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />

        <input
          type="text"
          v-model="store.formPatient.lastName"
          placeholder="Lastname"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />

        <input
          type="text"
          v-model="store.formPatient.middleName"
          placeholder="Middlename"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />

        <input
          type="password"
          v-model="store.formPatient.password"
          placeholder="Password"
          autocomplete="off"
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />

        <input
          type="email"
          v-model="store.formPatient.email"
          placeholder="Email"
          autocomplete="off"
          required
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
        />

        <vue-tel-input
          v-model="store.formPatient.contactNumber"
          class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
          style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
          :inputOptions="{
            class: 'w-full !px-0 !py-0 !rounded-none !border-none !text-gray-900 !transition',
            style: 'border-image: none',
            placeholder: 'Emergency Contact',
            type: 'tel',
            maxlength: 15,
            required: true
          }"
          :auto-default-country="false"
          auto-mode-dial-code
          mode="international"
          validCharactersOnly
        ></vue-tel-input>

        <div class="flex gap-3 mt-4">
          <button
            type="submit"
            class="flex-1 px-4 py-2 rounded-[5px] border-2 border-blue-600 text-blue-600 font-semibold hover:bg-blue-600 hover:text-white transition"
          >
            Update Patient
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
import { usePatientStore } from '@/stores/patientsStore'
import { VueTelInput } from 'vue-tel-input';

const store = usePatientStore()

const emit = defineEmits(['close'])

const submitHandler = async () => {
  const success = await store.submitPatient()
  if (success) {
    emit('close')
  }
}
</script>