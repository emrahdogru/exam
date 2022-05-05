<template>
    <div>
        <div class="row">
            <h2>{{exam.title}}</h2>
            <div class="col">
                {{exam.content}}
            </div>
        </div>
        <div v-for="q in exam.questions" :key="q.id" :class="{ 'text-success': result.results && result.results[q.id], 'text-danger': result.results && !result.results[q.id] }">
            <div class="row">
                <div class="col">
                    <p>{{q.content}}</p>
                </div>
            </div>
            <div class="row">
                <div class="col-6" v-for="c in q.choices" :key="c.id">
                    <label>
                        <input type="radio" :name="'q_' + q.id" v-model="result.answers[q.id]" :value="c.id" />
                        {{c.text}}
                    </label>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col">
                    <p class="text-center">
                        <button class="btn btn-success" @click="getResult">Sınavı Tamamla</button>
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'
    export default {
        data() {
            return {
                exam: null,
                result: null
            }
        },
        created() {
            this.getData(this.$route.params.id);
        },
        methods: {
            getData(id) {
                axios.get('api/examine/' + id)
                    .then(x => {
                        this.exam = x.data;
                        this.result = {
                            id : this.exam.id,
                            answers: {}
                        }
                    })
                    .catch(() => {
                        alert('Sınav yüklenemedi.');
                    })
            },
            getResult() {
                axios.post('api/examine', this.result)
                    .then(x => {
                        this.result = x.data;
                    })
                    .catch(x => {
                        console.error(x);
                        alert('Sınav sonucu değerlendirilemedi.');
                    })
            }
        }
    }
</script>