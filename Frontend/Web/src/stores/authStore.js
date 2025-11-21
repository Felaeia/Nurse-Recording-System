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
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formLogin.value),
      })

      const data = await response.json().catch((e) => {
        console.error('Failed to parse JSON response:', e)
        return { message: 'Received non-JSON response from server.' }
      })

      console.log('API Raw Response:', data)

      // FIX: The schema has 'user' and 'token', not 'success' or 'nurse'
      // We check if data.user exists and if the API says they are authenticated
      if (data.user && data.user.isAuthenticated) {
        
        // Map the API's 'user' object to your app's 'nurse' state
        nurse.value = data.user
        
        // Store the token separately if needed, or just store the whole nurse object
        // For now, we store the user object as 'nurse' in local storage
        localStorage.setItem('nurse', JSON.stringify(nurse.value))
        
        console.log(`Logged In as ${data.user.email}`)
        resetFormLogin()
        return true
      } else {
        console.error('Login failed:', data.message || 'Unknown error')
        resetFormLogin()
        return false
      }
    } catch (error) {
      console.error('Error during login:', error)
      resetFormLogin()
      return false
    }
  }

  const logout = () => {
    nurse.value = null
    localStorage.removeItem('nurse')
    console.log(`Logged Out`)
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