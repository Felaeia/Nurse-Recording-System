<template>
  <div>
    <form @submit.prevent="submitHandler">
      <label for="symptom">Symptom: </label>
      <textarea
        name="symptom"
        id="symptom"
        v-model="patientRecordStore.recordForm.symptom"
      ></textarea>

      <label for="diagnosis">Diagnosis: </label>
      <textarea
        name="diagnosis"
        id="diaganosis"
        v-model="patientRecordStore.recordForm.diagnosis"
      ></textarea>

      <label for="notes">Notes: </label>
      <textarea name="notes" id="notes" v-model="patientRecordStore.recordForm.notes"></textarea>

      <label for="treatment">Possible Treatment: </label>
      <textarea
        name="treatment"
        id="treatment"
        v-model="patientRecordStore.recordForm.treatment"
      ></textarea>

      <button type="submit">
        {{ patientRecordStore.isEditMode ? 'Update Record' : 'Add Record' }}
      </button>
      <button v-if="patientRecordStore.isEditMode" @click="patientRecordStore.resetRecordForm()">
        Cancel
      </button>
    </form>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { usePatientRecord } from '@/stores/patientRecord'

const patientRecordStore = usePatientRecord()
const route = useRoute()

const submitHandler = () => {
  patientRecordStore.recordForm.patientId = String(route.params.id)
  patientRecordStore.recordForm.date = new Date().toISOString().split('T')[0]
  patientRecordStore.submitRecord()
}

onMounted(() => {
  patientRecordStore.resetRecordForm()
})
</script>

<style lang="scss" scoped></style>
