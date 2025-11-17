import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import Patientview from '../views/PatientView.vue'
import PatientRecords from '@/views/PatientRecords.vue'
import LoginView from '@/views/LoginView.vue'
import AppointmentView from '@/views/AppointmentView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: Patientview,
      meta: { requiresAuth: true },
    },
    {
      path: '/patientrecords/:id',
      name: 'patientrecords',
      component: PatientRecords,
      meta: { requiresAuth: true },
    },
    {
      path: '/appointments',
      name: 'appointments',
      component: AppointmentView,
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/print/:patientId/:recordId?',
      name: 'print',
      component: () => import('../views/PrintView.vue'),
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  const isAuthenticated = authStore.isAutheticated

  if (to.meta.requiresAuth && !isAuthenticated) {
    next({ name: 'login' })
    return
  }

  if (to.name === 'login' && isAuthenticated) {
    next({ name: 'home' })
    return
  }

  next()
})

export default router
