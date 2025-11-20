<template>
  <div
    class="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-50 via-blue-50/30 to-red-50/20 font-poppins p-4"
  >
    <div class="w-full max-w-md">
      <!-- Logo and Title Section -->
      <div class="text-center mb-10 animate-fade-in">
        <div class="gradient-logo w-32 h-32 mx-auto mb-6 drop-shadow-2xl animate-float"></div>
        <h1
          class="text-5xl font-black tracking-tight mb-2 bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent animate-slide-down"
        >
          ACLC CLINIC
        </h1>
        <p class="text-gray-500 text-sm font-medium animate-slide-up">
          Healthcare Management System
        </p>
      </div>

      <!-- Login Form Card -->
      <div class="bg-white rounded-2xl shadow-2xl p-8 border border-gray-100 animate-scale-in">
        <div class="mb-6">
          <h2 class="text-2xl font-bold text-gray-800 mb-1">Welcome Back</h2>
          <p class="text-sm text-gray-500">Sign in to access your account</p>
        </div>

        <form @submit.prevent="submitHandler" class="space-y-6">
          <!-- Username Field -->
          <div class="form-group">
            <label for="username" class="block text-sm font-semibold text-gray-700 mb-2 text-left">
              Username:
            </label>
            <div class="relative">
              <div class="absolute left-4 top-1/2 -translate-y-1/2 pointer-events-none">
                <i class="fa-solid fa-user text-gray-400"></i>
              </div>
              <input
                type="text"
                id="username"
                v-model="auth.formLogin.username"
                placeholder="Enter your username"
                class="w-full pl-12 pr-4 py-3.5 rounded-xl focus:outline-none bg-white text-gray-800 placeholder-gray-400 transition-all duration-300 border-2 border-gray-200 hover:border-gray-300 gradient-border-focus"
                required
              />
            </div>
          </div>

          <!-- Password Field -->
          <div class="form-group">
            <label for="password" class="block text-sm font-semibold text-gray-700 mb-2 text-left">
              Password:
            </label>
            <div class="relative">
              <div class="absolute left-4 top-1/2 -translate-y-1/2 pointer-events-none">
                <i class="fa-solid fa-lock text-gray-400"></i>
              </div>
              <input
                type="password"
                id="password"
                v-model="auth.formLogin.password"
                placeholder="Enter your password"
                class="w-full pl-12 pr-4 py-3.5 rounded-xl focus:outline-none bg-white text-gray-800 placeholder-gray-400 transition-all duration-300 border-2 border-gray-200 hover:border-gray-300 gradient-border-focus"
                required
              />
            </div>
          </div>

          <!-- Login Button -->
          <button
            type="submit"
            class="w-full py-4 text-white text-base font-bold rounded-xl transition-all hover:shadow-lg hover:scale-[1.02] active:scale-95 mt-8 bg-gradient-to-r from-[#2933FF] to-[#FF5451] border-2 border-transparent hover:border-[#2933FF]"
          >
            <i class="fa-solid fa-arrow-right-to-bracket mr-2"></i>
            Login
          </button>
        </form>

        <!-- Additional Info -->
        <div class="mt-6 pt-6 border-t border-gray-100">
          <p class="text-center text-xs text-gray-500">
            <i
              class="fa-solid fa-shield-halved mr-1 bg-gradient-to-r from-[#2933FF] to-[#FF5451] bg-clip-text text-transparent"
            ></i>
            Secure login protected by encryption
          </p>
        </div>
      </div>

      <!-- Footer -->
      <div class="text-center mt-8 animate-fade-in-delayed">
        <p class="text-sm text-gray-500">© 2024 ACLC Clinic. All rights reserved.</p>
      </div>
    </div>
  </div>
</template>

<script setup>
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'

const auth = useAuthStore()

const submitHandler = async () => {
  const success = await auth.login()
  if (success) {
    router.push({ name: 'home' })
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700;900&display=swap');

.gradient-logo {
  mask: url('../../images/ACLC.svg') center / contain no-repeat;
  -webkit-mask: url('../../images/ACLC.svg') center / contain no-repeat;
  background: linear-gradient(135deg, #2933ff, #ff5451, #8b5cf6);
  background-size: 200% 200%;
  width: 8rem;
  height: 8rem;
  display: inline-block;
  filter: drop-shadow(0 8px 16px rgba(41, 51, 255, 0.3));
  animation: gradientMove 5s ease infinite;
}

@keyframes gradientMove {
  0% {
    background-position: 0% 50%;
  }
  50% {
    background-position: 100% 50%;
  }
  100% {
    background-position: 0% 50%;
  }
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-10px);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes scaleIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

.animate-float {
  animation: float 3s ease-in-out infinite;
}

.animate-fade-in {
  animation: fadeIn 0.8s ease-out;
}

.animate-fade-in-delayed {
  animation: fadeIn 1s ease-out 0.3s both;
}

.animate-slide-down {
  animation: slideDown 0.6s ease-out 0.2s both;
}

.animate-slide-up {
  animation: slideUp 0.6s ease-out 0.3s both;
}

.animate-scale-in {
  animation: scaleIn 0.5s ease-out 0.4s both;
}

.gradient-border-focus:focus {
  border-image: linear-gradient(135deg, #2933ff, #ff5451) 1;
  border-width: 2px;
  border-style: solid;
}
</style>
