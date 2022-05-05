import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
    {
        name: 'home',
        path: '/',
        component: () => import('../views/Home.vue')
    },
    {
        name: 'login',
        path: '/login',
        component: () => import('../views/Login.vue')
    },
    {
        name: 'logout',
        path: '/logout',
        component: () => import('../views/Logout.vue')
    },
    {
        name: 'examinations',
        path: '/examinations',
        component: () => import('../views/ExaminationList.vue')
    },
    {
        name: 'newexamination',
        path: '/newexamination',
        component: () => import('../views/ExaminationNew.vue')
    },
]

const router = new VueRouter({
    mode: 'history',
    linkActiveClass: 'active',
    routes,
})

export default router