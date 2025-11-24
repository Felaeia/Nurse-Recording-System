<template>
  <div class="print-container font-poppins">
    <!-- Screen View (with buttons) -->
    <div class="screen-only bg-gradient-to-br from-gray-50 via-blue-50/30 to-red-50/20 min-h-screen p-8">
      <div class="max-w-5xl mx-auto">
        <div class="flex justify-between items-center mb-6">
          <button
            @click="goBack"
            class="px-6 py-3 bg-gray-600 text-white rounded-xl font-semibold hover:bg-gray-700 transition-all flex items-center gap-2"
          >
            <i class="fa-solid fa-arrow-left"></i>
            Back
          </button>
          <button
            @click="print"
            class="px-6 py-3 bg-gradient-to-r from-[#2933FF] to-[#FF5451] text-white rounded-xl font-semibold hover:shadow-lg transition-all flex items-center gap-2"
          >
            <i class="fa-solid fa-print"></i>
            Print Document
          </button>
        </div>

        <!-- Print Preview -->
        <div class="print-content bg-white shadow-2xl">
          <PrintContent />
        </div>
      </div>
    </div>

    <!-- Print Only Content (hidden on screen) -->
    <div class="print-only">
      <PrintContent />
    </div>
  </div>
</template>

<script setup>
import { useRouter, useRoute } from 'vue-router'
import { usePrintStore } from '@/stores/printStore'
import { onMounted, onUnmounted, defineComponent, h } from 'vue'

const router = useRouter()
const route = useRoute()
const printStore = usePrintStore()

// Print Content Component
const PrintContent = defineComponent({
  setup() {
    return () => h('div', { class: 'p-12' }, [
      // Header
      h('div', { class: 'header text-center mb-12 pb-8 border-b-2 border-gray-300' }, [
        h('div', { class: 'flex items-center justify-center gap-4 mb-4' }, [
          h('div', {
            class: 'gradient-logo w-20 h-20',
            style: {
              mask: 'url(/images/ACLC.svg) center / contain no-repeat',
              WebkitMask: 'url(/images/ACLC.svg) center / contain no-repeat',
              background: 'linear-gradient(135deg, #2933ff, #ff5451)'
            }
          })
        ]),
        h('h1', { class: 'text-4xl font-bold text-gray-800 mb-2' }, 'ACLC CLINIC'),
        h('p', { class: 'text-gray-600 text-sm' }, 'Healthcare Management System'),
        h('p', { class: 'text-gray-500 text-xs mt-2' }, 'User-Submitted Forms')
      ]),

      // Document Title
      h('div', { class: 'mb-8 text-center' }, [
        h('h2', { class: 'text-2xl font-bold text-gray-800 mb-2' }, 'USER FORM'),
        h('p', { class: 'text-gray-600' }, `Date Issued: ${printStore.todaysDate}`)
      ]),

      // Patient Information
      h('div', { class: 'patient-info mb-8 bg-gray-50 p-6 rounded-lg' }, [
        h('h3', { class: 'text-xl font-bold text-gray-800 mb-4 pb-2 border-b border-gray-300' }, 'PATIENT INFORMATION'),
        h('div', { class: 'grid grid-cols-2 gap-4 text-sm' }, [
          h('div', [
            h('p', { class: 'text-gray-600 font-semibold mb-1' }, 'Full Name:'),
            h('p', { class: 'text-gray-800' }, printStore.patientFullName)
          ]),
          h('div', [
            h('p', { class: 'text-gray-600 font-semibold mb-1' }, 'Patient ID:'),
            h('p', { class: 'text-gray-800' }, `#${printStore.patient?.userId || 'N/A'}`)
          ]),
          h('div', [
            h('p', { class: 'text-gray-600 font-semibold mb-1' }, 'Contact Number:'),
            h('p', { class: 'text-gray-800' }, printStore.patient?.contactNumber || 'N/A')
          ]),
          h('div', [
            h('p', { class: 'text-gray-600 font-semibold mb-1' }, 'Email Address:'),
            h('p', { class: 'text-gray-800' }, printStore.patient?.email || 'N/A')
          ]),
          h('div', { class: 'col-span-2' }, [
            h('p', { class: 'text-gray-600 font-semibold mb-1' }, 'Address:'),
            h('p', { class: 'text-gray-800' }, printStore.patient?.address || 'Not Provided')
          ])
        ])
      ]),

      // User Forms
      h('div', { class: 'user-forms mb-12' }, [
        h('h3', { class: 'text-xl font-bold text-gray-800 mb-4 pb-2 border-b border-gray-300' }, 'USER FORMS'),
        ...printStore.forms.map((form, index) =>
          h('div', {
            class: 'form-entry mb-6 p-5 border border-gray-200 rounded-lg',
            key: form.formId
          }, [
            h('div', { class: 'flex justify-between items-start mb-4' }, [
              h('div', [
                h('h4', { class: 'text-lg font-bold text-gray-800' }, `Form #${index + 1}`),
                h('p', { class: 'text-xs text-gray-500' }, `Form ID: ${form.formId}`)
              ]),
            ]),

            h('div', { class: 'space-y-3 text-sm' },
              Object.entries(form).map(([key, value]) =>
                h('div', [
                  h('p', { class: 'font-semibold text-gray-700 mb-1' }, `${key}:`),
                  h('p', { class: 'text-gray-800' }, value)
                ])
              )
            )
          ])
        ),
        printStore.forms.length === 0
          ? h('p', { class: 'text-center text-gray-500 py-8' }, 'No forms available for this patient.')
          : null
      ]),

      // Professional Statement
      h('div', { class: 'statement mb-8 p-5 bg-blue-50 border-l-4 border-blue-600 rounded' }, [
        h('p', { class: 'text-sm text-gray-700 leading-relaxed' }, [
          'This document contains user-submitted form data. ',
          'All information contained herein is confidential and should be handled in accordance with applicable privacy regulations. '
        ])
      ]),

      // Approval Section
      h('div', { class: 'approval mt-12 pt-8 border-t-2 border-gray-300' }, [
        h('div', { class: 'grid grid-cols-2 gap-8' }, [
          h('div', [
            h('p', { class: 'text-sm font-semibold text-gray-700 mb-2' }, 'Prepared By:'),
            h('p', { class: 'text-gray-800 font-medium mb-1' }, printStore.nurse?.userName || 'N/A'),
            h('p', { class: 'text-xs text-gray-600' }, printStore.nurse?.email || ''),
            h('div', { class: 'mt-4 pt-2 border-t border-gray-400 w-48' }),
            h('p', { class: 'text-xs text-gray-600 mt-1' }, 'Nurse Signature')
          ]),
          h('div', [
            h('p', { class: 'text-sm font-semibold text-gray-700 mb-2' }, 'Date Prepared:'),
            h('p', { class: 'text-gray-800' }, printStore.todaysDate),
            h('div', { class: 'mt-8' })
          ])
        ])
      ]),

      // Footer
      h('div', { class: 'footer text-center mt-12 pt-8 border-t border-gray-200 text-xs text-gray-500' }, [
        h('p', { class: 'mb-1' }, '© 2024 ACLC Clinic. All rights reserved.'),
        h('p', 'This document is confidential and intended solely for authorized personnel.')
      ])
    ])
  }
})

onMounted(() => {
  const patientId = Number(route.params.patientId)
  const formId = route.params.recordId ? Number(route.params.recordId) : null
  
  printStore.setPrintData(patientId, formId)
})

onUnmounted(() => {
  printStore.resetPrintData()
})

const goBack = () => {
  router.back()
}

const print = () => {
  printStore.printDocument()
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

/* Screen-only styles */
.screen-only {
  display: block;
}

.print-only {
  display: none;
}

/* Print-specific styles */
@media print {
  .screen-only {
    display: none !important;
  }

  .print-only {
    display: block !important;
  }

  .print-container {
    background: white !important;
  }

  /* Remove any shadows, borders that don't print well */
  * {
    -webkit-print-color-adjust: exact !important;
    print-color-adjust: exact !important;
  }

  /* Page breaks */
  .record-entry {
    page-break-inside: avoid;
  }

  .approval {
    page-break-before: avoid;
  }

  /* Ensure proper margins for printing */
  @page {
    margin: 1.5cm;
  }
}

.print-content {
  background: white;
  min-height: 100vh;
}

.gradient-logo {
  display: inline-block;
}
</style>