import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('authStore', () => {
  const storedNurse = localStorage.getItem('nurse')
  const nurse = ref(storedNurse ? JSON.parse(storedNurse) : null)

  const mockNurse = ref({
    id: 1,
    username: 'aclcnurse',
    password: 'aclcnurse123',
    email: 'aclcnurse@gmail.com',
  })

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
    if (
      formLogin.value.username === mockNurse.value.username &&
      formLogin.value.password === mockNurse.value.password
    ) {
      localStorage.setItem('nurse', JSON.stringify(mockNurse))
      console.log(`Successfully Logged`)
      return true
    } else {
      console.error(`Failed Logged`)
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
