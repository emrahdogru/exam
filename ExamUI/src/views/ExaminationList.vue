<template>
    <div>
        <router-link to="/newexamination" class="float-end">Yeni Sınav Oluştur</router-link>
        <h1>Sınav Listesi</h1>
        <div v-if="error" class="alert alert-danger">
            Sınav listesi yüklenemedi.
        </div>
        <div v-else-if="list===null">Yükleniyor...</div>
        <div v-else>
            <table class="table">
                <thead>
                    <tr>
                        <th>Başlık</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody v-if="list.length">
                    <tr v-for="e in list" :key="e.Id">
                        <td><router-link to="detay">{{e.title}}</router-link></td>
                        <td class="text-right"><router-link to="sil">Sil</router-link></td>
                    </tr>
                </tbody>
                <tbody v-else>
                    <tr>
                        <td colspan="2" class="text-center">
                            <i>Kayıtlı sınav yok.</i>
                            <div>
                                <router-link to="/newexamination">Yeni Sınav Oluştur</router-link>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'
    export default {
        data() {
            return {
                list: null,
                error: null
            }
        },
        created() {
            axios.get('api/examinations')
                .then(x => {
                    this.list = x.data;
                })
                .catch(x => {
                    console.error(x);
                    this.error = 'Sınav listesi yüklenemedi.';
                })
        }
    }
</script>