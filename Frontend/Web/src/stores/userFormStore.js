import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useUserFormStore = defineStore('userFormStore', () => {
  const forms = ref([])
  const loading = ref(false)
  const error = ref(null)
  const formToEdit = ref(null)

  const fetchUserForms = async (userId) => {
    loading.value = true
    error.value = null
    try {
      const response = await fetch(`https://localhost:7031/api/NurseUserForm/user/form_list?userId=${userId}`, {
        method: 'GET',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' }
      })
      if (!response.ok) throw new Error('Failed to fetch user forms')
      forms.value = await response.json()
    } catch (err) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  const setFormForEdit = (form) => {
    formToEdit.value = form
  }

  const updateUserForm = async (updatedForm, userId) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'))
      const nurseId = nurseData?.nurseDetails?.nurseId

      if (!nurseId) {
        throw new Error('Nurse details not found. Please log in again.')
      }

      const payload = {
        formId: updatedForm.formId,
        issueType: updatedForm.issueType,
        issueDescryption: updatedForm.issueDescryption,
        status: updatedForm.status,
        patientName: updatedForm.patientName,
      }

      const response = await fetch(`https://localhost:7031/api/UserForm/update/user_form`, {
        method: 'PUT',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          'X-UpdatedBy': nurseId,
        },
        body: JSON.stringify(payload),
      })
      if (!response.ok) throw new Error('Failed to update user form')
      await fetchUserForms(userId)
    } catch (err) {
      console.error('Error updating user form:', err)
    }
  }

  const deleteUserForm = async (formId, userId) => {
    try {
      const nurseData = JSON.parse(localStorage.getItem('nurse'))
      const nurseId = nurseData?.nurseDetails?.nurseId

      if (!nurseId) {
        throw new Error('Nurse details not found. Please log in again.')
      }

      const response = await fetch(`https://localhost:7031/api/UserForm/delete/user_form/${formId}`, {
        method: 'DELETE',
        credentials: 'include',
        headers: {
          'X-DeletedBy': nurseId,
        },
      })
      if (!response.ok) throw new Error('Failed to delete user form')
      await fetchUserForms(userId)
    } catch (err) {
      console.error('Error deleting user form:', err)
    }
  }

  return {
    forms,
    loading,
    error,
    formToEdit,
    fetchUserForms,
    setFormForEdit,
    updateUserForm,
    deleteUserForm,
  }
})
