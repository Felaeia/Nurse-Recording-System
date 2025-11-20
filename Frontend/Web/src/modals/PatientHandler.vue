<template>
  <form @submit.prevent="submitHandler" class="max-w-md mx-auto p-6 rounded-xl space-y-4">
    <input
      type="text"
      v-model="store.formPatient.firstname"
      placeholder="Firstname"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="text"
      v-model="store.formPatient.lastname"
      placeholder="Lastname"
      autocomplete="off"
      required
      class="w-full px-3 py-2 rounded-[5px] border-2 text-gray-900 transition"
      style="border-image: linear-gradient(135deg, #2933ff, #ff5451) 1; border-style: solid"
    />

    <input
      type="text"
      v-model="store.formPatient.middlename"
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
      type="text"
      v-model="store.formPatient.facebook"
      placeholder="Facebook"
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

    <input
      type="number"
      v-model="store.formPatient.emergencyContact"
      placeholder="Emergency Contact"
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
        {{ store.isEditMode ? 'Update Patient' : 'Add Patient' }}
      </button>
      <button
        v-if="store.isEditMode"
        type="button"
        @click="store.resetForm"
        class="flex-1 px-4 py-2 rounded-[5px] border-2 border-gray-400 text-gray-600 font-semibold hover:bg-gray-400 hover:text-white transition"
      >
        Cancel
      </button>
    </div>
  </form>
</template>

<script setup>
import { usePatientStore } from '@/stores/patientsStore'

const store = usePatientStore()

const submitHandler = () => {
  store.submitPatient()
}
</script>
