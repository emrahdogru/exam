<template>
    <div class="row">
        <div class="col">

        </div>
        <div class="col">
            <form v-on:submit.prevent="login">
                <div class="mb-3">
                    <label for="username" class="form-label">Kullanıcı adı</label>
                    <input type="text" v-model="form.username" class="form-control" id="username">
                </div>
                <div class="mb-3">
                    <label for="parola" class="form-label">Parola</label>
                    <input type="password" v-model="form.password" class="form-control" id="parola">
                </div>

                <button type="submit" class="btn btn-primary">Giriş</button>
            </form>
        </div>
        <div class="col">

        </div>
    </div>
</template>
<script>
    import axios from 'axios'

    export default {
        data() {
            return {
                form: {
                    username: '',
                    password: ''
                }
            }
        },
        methods: {
            login() {
                axios.post('api/login', this.form)
                    .then(x => {
                        console.log('Login result: ', x.data);

                        window.localStorage.setItem('user', JSON.stringify(x.data));

                        this.$root.$data.login = x.data;
                        axios.defaults.headers.common['authorization'] = 'bearer ' + x.data.Token;
                        this.$router.push('/');
                    }).
                    catch (x => {
                        console.log(x);
                    })
            }
        }
    }
</script>