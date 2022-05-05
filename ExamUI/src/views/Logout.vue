<template>
    <div  class="text-center">
        <h3>Oturum sonland?</h3>
        <router-link to="/">Giri?</router-link>
    </div>
    
</template>
<script>
    import axios from 'axios'
    export default {
        created() {
            window.localStorage.removeItem('user');
            this.$root.$data.login = null;
            axios.defaults.headers.common['authorization'] = null;
            location.reload();
        }
    }
</script>