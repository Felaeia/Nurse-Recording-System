import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('authStore', () => {
  const storedNurse = localStorage.getItem('nurse')
  const nurse = ref(storedNurse ? JSON.parse(storedNurse) : null)

  const formLogin = ref({
    email: '',
    password: '',
  })

  const resetFormLogin = () => {
    formLogin.value = {
      email: "",
      password: "",
    }
  }

  const isAutheticated = computed(() => !!nurse.value)

  const login = async () => {
    try {
      const response = await fetch('https://localhost:7031/api/Auth/login', {
        method: 'POST',
        credentials: 'include', 
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formLogin.value),
      })

      const data = await response.json()

      if (data.user && data.user.isAuthenticated) {
        // We only save the user INFO. The token is now safely hidden in a cookie.
        nurse.value = data.user
        localStorage.setItem('nurse', JSON.stringify(nurse.value))
        if (data.user.nurseDetails && data.user.nurseDetails.nurseId) {
          localStorage.setItem('nurseId', data.user.nurseDetails.nurseId);
        }
        return true
      } else {
        console.error('Login failed:', data.message)
        return false
      }
    } catch (error) {
      console.error('Error:', error)
      return false
    }
  }

  const logout = async () => {
    try {
    await fetch('https://localhost:7031/api/Auth/logout', {
      method: 'POST',
      credentials: 'include',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({}) 
    })
  } catch (error) {
    console.error('Logout error', error)
  } finally {
    // Always clear client state, even if server errors out
    nurse.value = null
    localStorage.removeItem('nurse')
    localStorage.removeItem('nurseId')
    // Optional: Redirect to login
  }
  }

  return {
    nurse,
    formLogin,
    isAutheticated,
    login,
    logout,
    resetFormLogin,
  }
})