<template>
  <div class="sidebar">
    <!-- <SidebarComponent /> -->
    <PatientHandler />
  </div>
  <div class="main">
    <div class="search">
      <input type="text" v-model="store.searchterm" placeholder="Patient Name..." />
    </div>
    <div class="content">
      <div class="patients" v-for="patient in store.filteredpatients">
        <div :key="patient.id" @click="patientsrecord(patient.id)">
          <p class="firstname">{{ patient.firstname }}</p>
          <p class="middlename">{{ patient.middlename }}</p>
          <p class="lastname">{{ patient.lastname }}</p>
          <p class="contactnumber">{{ patient.emergencyContact }}</p>
          <p class="adress">{{ patient.email }}</p>
        </div>
        <button @click="handleEdit(patient)">Edit</button>
        <button @click="handleDelete(patient.id)">Delete</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
import { usePatientStore } from '@/stores/patientsStore'
import SidebarComponent from '@/components/SidebarComponent.vue'
import PatientHandler from '@/modals/PatientHandler.vue'

const store = usePatientStore()
const router = useRouter()

const patientsrecord = (id) => {
  router.push({ name: 'patientrecords', params: { id } })
}

const handleEdit = (patient) => {
  store.setFormforEdit(patient)
}

const handleDelete = (id) => {
  store.deletePatient(id)
}
</script>

<style lang="scss" scoped></style>
