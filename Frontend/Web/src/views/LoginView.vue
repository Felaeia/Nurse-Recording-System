<template>
  <div>
    <form @submit.prevent="submitHandler">
      <label for="username">Username: </label>
      <input type="text" id="username" v-model="auth.formLogin.username" />

      <label for="password">Password: </label>
      <input type="text" id="password" v-model="auth.formLogin.password" />

      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/authStore'

const auth = useAuthStore()

const submitHandler = () => {
  auth.login()
}
</script>

<style lang="scss" scoped></style>
