<template>
  <div
    class="min-h-screen bg-gradient-to-br from-gray-50 via-blue-50/30 to-red-50/20 font-poppins p-10"
  >
    <div class="max-w-7xl mx-auto">
      <div class="Personalinfo bg-white rounded-2xl shadow-lg p-8 mb-8 border border-gray-100">
        <div class="flex items-center justify-between">
          <div class="flex items-center gap-4">
            <div
              class="w-16 h-16 rounded-full bg-gradient-to-r from-[#2933FF] to-[#FF5451] flex items-center justify-center text-white text-2xl font-bold shadow-lg"
            >
              {{ patient?.firstName?.[0] }}{{ patient?.lastName?.[0] }}
            </div>
            <div>
              <h2
                class="text-3xl font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
              >
                {{ patient?.firstName }} {{ patient?.lastName }}
              </h2>
              <p class="text-gray-500 text-sm mt-1">User Forms</p>
            </div>
          </div>
          <div class="flex gap-3">
            <button
              @click="printAllForms"
              class="px-6 py-3 bg-gradient-to-r from-purple-500 to-purple-600 text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95 flex items-center gap-2"
            >
              <i class="fa-solid fa-print"></i>
              Print All Forms
            </button>
            <button
              @click="goBack"
              class="px-6 py-3 bg-gradient-to-r from-gray-500 to-gray-600 text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95 flex items-center gap-2"
            >
              <i class="fa-solid fa-arrow-left"></i>
              Back to Patient List
            </button>
          </div>
        </div>
      </div>

      <div class="user-forms">
        <div v-if="userFormStore.loading" class="text-center py-12">
          <p class="text-gray-500 text-lg">Loading...</p>
        </div>
        <div v-else-if="userFormStore.error" class="text-center py-12">
          <p class="text-red-500 text-lg">{{ userFormStore.error }}</p>
        </div>
        <div v-else-if="userFormStore.forms.length === 0" class="text-center py-12">
          <div
            class="w-20 h-20 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center mx-auto mb-4"
          >
            <i
              class="fa-solid fa-folder-open text-3xl bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
            ></i>
          </div>
          <p class="text-gray-500 text-lg">No forms found for this user.</p>
        </div>
        <div v-else class="forms-grid grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          <div
            v-for="form in userFormStore.forms"
            :key="form.formId"
            class="bg-white rounded-2xl p-6 shadow-lg hover:shadow-2xl transition-all hover:-translate-y-2 duration-300 border border-gray-100 group relative overflow-hidden"
          >
            <div class="relative z-10">
              <div class="flex items-start justify-between mb-4">
                <h2 class="text-xl font-bold bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent">
                  Form #{{ form.formId }}
                </h2>
                <div class="flex gap-2">
                  <button
                    @click="printSingleForm(form.formId)"
                    class="w-8 h-8 rounded-lg bg-purple-50 flex items-center justify-center hover:shadow-md transition-all hover:scale-110 active:scale-95"
                    title="Print Form"
                  >
                    <i class="fa-solid fa-print text-xs text-purple-600"></i>
                  </button>
                  <button
                    @click="handleEdit(form)"
                    class="w-8 h-8 rounded-lg bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center hover:shadow-md transition-all hover:scale-110 active:scale-95"
                    title="Edit Form"
                  >
                    <i
                      class="fa-solid fa-pen-to-square text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
                    ></i>
                  </button>
                  <button
                    @click="confirmDelete(form)"
                    class="w-8 h-8 rounded-lg bg-red-50 flex items-center justify-center hover:shadow-md transition-all hover:scale-110 active:scale-95"
                    title="Delete Form"
                  >
                    <i class="fa-solid fa-trash text-xs text-red-500"></i>
                  </button>
                </div>
              </div>
              <div class="space-y-3">
                <div v-for="(value, key) in form" :key="key" class="flex items-start gap-2">
                  <span class="w-8 h-8 rounded-full bg-gradient-to-r from-[#2933FF]/10 to-[#FF5451]/10 flex items-center justify-center flex-shrink-0 mt-1">
                    <i class="fa-solid fa-file-alt text-xs bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"></i>
                  </span>
                  <div>
                    <h3 class="text-sm font-semibold text-gray-700 mb-1">{{ key }}</h3>
                    <p class="text-sm text-gray-600">{{ value }}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <EditUserFormModal v-if="showEditModal" :form="formToEdit" :userId="userId" @close="showEditModal = false" />
    <div
      v-if="showDeleteModal"
      class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4 font-poppins"
    >
      <div class="bg-white rounded-2xl shadow-2xl max-w-md w-full border border-gray-100 p-6">
        <div class="flex items-center gap-3 mb-4">
          <div class="w-12 h-12 rounded-full bg-red-50 flex items-center justify-center">
            <i class="fa-solid fa-exclamation-triangle text-xl text-red-500"></i>
          </div>
          <div>
            <h3 class="text-xl font-bold text-gray-800">Delete Form</h3>
            <p class="text-sm text-gray-500">This action cannot be undone</p>
          </div>
        </div>
        <p class="text-gray-600 mb-6">
          Are you sure you want to delete form #{{ formToDelete?.formId }}?
        </p>
        <div class="flex justify-end gap-3">
          <button
            @click="cancelDelete"
            class="px-6 py-3 bg-gray-100 text-gray-700 text-sm font-semibold rounded-xl transition-all hover:bg-gray-200 hover:shadow-md active:scale-95"
          >
            Cancel
          </button>
          <button
            @click="handleDelete"
            class="px-6 py-3 bg-gradient-to-r from-red-500 to-red-600 text-white text-sm font-semibold rounded-xl transition-all hover:shadow-lg hover:scale-105 active:scale-95"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router'
import { usePatientStore } from '@/stores/patientsStore'
import { useUserFormStore } from '@/stores/userFormStore'
import { computed, onMounted, ref } from 'vue'
import EditUserFormModal from '@/modals/EditUserFormModal.vue'

const route = useRoute()
const router = useRouter()
const patientsStore = usePatientStore()
const userFormStore = useUserFormStore()

const userId = Number(route.params.id)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const formToEdit = ref(null)
const formToDelete = ref(null)

const patient = computed(() => {
  return patientsStore.patients.find((p) => p.userId === userId)
})

onMounted(() => {
  userFormStore.fetchUserForms(userId)
})

const goBack = () => {
  router.push({ name: 'home' })
}

const handleEdit = (form) => {
  formToEdit.value = form
  showEditModal.value = true
}

const confirmDelete = (form) => {
  formToDelete.value = form
  showDeleteModal.value = true
}

const cancelDelete = () => {
  formToDelete.value = null
  showDeleteModal.value = false
}

const handleDelete = async () => {
  if (formToDelete.value) {
    await userFormStore.deleteUserForm(formToDelete.value.formId, userId)
    showDeleteModal.value = false
    formToDelete.value = null
  }
}

const printSingleForm = (formId) => {
  router.push({
    name: 'print',
    params: {
      patientId: userId,
      recordId: formId,
    },
  })
}

const printAllForms = () => {
  router.push({
    name: 'print',
    params: {
      patientId: userId,
    },
  })
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
</style>