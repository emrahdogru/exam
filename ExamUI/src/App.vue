<template>
    <div class="container" id="app">
        <h1>APP</h1>
        <router-view />
    </div>
</template>

<script>
    import axios from 'axios'

    export default {
        name: 'app',
        components: {
            
        },
        beforeCreate() {
            var user = window.localStorage.getItem('user');

            if (user) {
                user = JSON.parse(user);
                console.log('Oturumu aç?k kullan?c?: ', user);
                this.$root.$data.login = user;
                axios.defaults.headers.common['authorization'] = 'bearer ' + user.token;
            } else {
                this.$router.push('/login')
            }
        },
    };
</script>

<style>

</style>

