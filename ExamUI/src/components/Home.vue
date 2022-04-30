<template>
    <div class="home">
        <h1>{{ msg }}</h1>
        <p>Welcome to your new single-page application, built with <a href="https://vuejs.org" target="_blank">Vue.js</a>.</p>
    </div>
</template>

<script>
    import axios from 'axios'
    export default {
        name: 'Home',
        props: {
            msg: String
        },
        created() {
            this.getData();
        },
        methods: {
            getData() {
                axios.get('https://localhost:44366/WeatherForecast')
                    .then(x => {
                        console.log(x);
                    })
                    .catch(x => {
                        console.error(x);
                    })
            }
        }
    };
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>

