import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import Patientview from '../views/PatientView.vue'
import PatientRecords from '@/views/PatientRecords.vue'
import LoginView from '@/views/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Patientview,
      // meta: { requiresAuth: true }
    },
    {
      path: '/patientrecords/:id',
      name: 'patientrecords',
      component: PatientRecords,
      // meta: { requiresAuth: true }
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  if (to.meta.requiresAuth) {
    if (authStore.isAutheticated) {
      next()
    } else {
      next('/login')
    }
  } else {
    next()
  }
})

export default router
