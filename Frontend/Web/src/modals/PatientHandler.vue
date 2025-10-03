<template>
  <form @submit.prevent="submitHandler">
    <label for="patientFirstname">Firstname: </label>
    <input
      type="name"
      v-model="store.formPatient.firstname"
      id="patientFirstname"
      autocomplete="off"
      required
    />

    <label for="patientLastname">Lastname: </label>
    <input
      type="name"
      v-model="store.formPatient.lastname"
      id="patientLastname"
      autocomplete="off"
      required
    />

    <label for="patientMiddlename">Middlename: </label>
    <input
      type="name"
      v-model="store.formPatient.middlename"
      id="patientMiddlename"
      autocomplete="off"
      required
    />

    <label for="patientPassword">Password: </label>
    <input
      type="password"
      v-model="store.formPatient.password"
      id="patientPassword"
      autocomplete="off"
    />

    <label for="patientFacebook">Facebook: </label>
    <input
      type="facebook"
      v-model="store.formPatient.facebook"
      id="patientFacebook"
      autocomplete="off"
    />

    <label for="patientEmail">Email: </label>
    <input
      type="email"
      v-model="store.formPatient.email"
      id="patientEmail"
      autocomplete="off"
      required
    />

    <label for="patientEmergencycontact">Emergencycontact: </label>
    <input
      type="number"
      v-model="store.formPatient.emergencyContact"
      id="patientEmergencycontact"
      autocomplete="off"
      required
    />

    <button type="submit">{{ store.isEditMode ? 'Update Patient' : 'Add Patient' }}</button>
    <button v-if="store.isEditMode" @click="store.resetForm">Cancel</button>
  </form>
</template>

<script setup>
import { usePatientStore } from '@/stores/patientsStore'

const store = usePatientStore()

const submitHandler = () => {
  store.submitPatient()
}
</script>
