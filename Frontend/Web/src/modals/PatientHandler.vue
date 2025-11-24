<template>
  <form @submit.prevent="submitHandler" class="max-w-md mx-auto p-6 rounded-xl space-y-4">
    <input
      type="text"
      v-model="form.firstname"
      placeholder="Firstname"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="text"
      v-model="form.lastname"
      placeholder="Lastname"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="text"
      v-model="form.middlename"
      placeholder="Middlename"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="password"
      v-model="form.password"
      placeholder="Password"
      autocomplete="off"
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="email"
      v-model="form.email"
      placeholder="Email"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <vue-tel-input
      v-model="form.contactNumber"
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
        Add Patient
      </button>
    </div>
  </form>
</template>

<script setup>
import { ref } from 'vue'
import { usePatientStore } from '@/stores/patientsStore'
import { VueTelInput } from 'vue-tel-input';

const store = usePatientStore()

const form = ref({
  firstname: '',
  lastname: '',
  middlename: '',
  password: '',
  email: '',
  contactNumber: '',
})

const submitHandler = async () => {
  const success = await store.addPatient(form.value)
  if (success) {
    form.value = {
      firstname: '',
      lastname: '',
      middlename: '',
      password: '',
      email: '',
      contactNumber: '',
    }
  }
}
</script>
