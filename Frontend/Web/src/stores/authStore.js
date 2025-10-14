import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('authStore', () => {
  const nurse = ref([])

  const formLogin = ref({
    username: '',
    password: '',
  })

  const resetFormLogin = () => {
    formLogin.value = {
      username: '',
      password: '',
    }
  }

  const isAutheticated = computed(() => !!nurse.value)

  const login = async () => {
    try {
      const response = await fetch('http:/localhost:3000/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formLogin.value),
      })

      const data = await response.json()

      if (data.success) {
        nurse.value = data.nurse
        ;(localStorage.setIte, ('nurse', JSON.stringify(nurse.value)))
        console.log(`Logged In as ${data.nurse.username}`)
        resetFormLogin()
        return true
      } else {
        console.error('Login failed:', data.message)
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
