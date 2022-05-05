<template>
    <div>
        <div class="row">
            <div class="col-2">
                Makale:
            </div>
            <div class="col-10">
                <select v-model="selectedContent" class="form-control" v-on:change="loadContent">
                    <option v-for="c in wiredContent" :key="c.id" :value="c">{{c.title}}</option>
                </select>
            </div>
        </div>
        <div class="row">
            <iframe ref="contentFrame" style=" height:300px;" class="form-control"></iframe>
        </div>
        <div class="row">
            <div class="col">
                <ol>
                    <li v-for="(q, index) in exam.questions" :key="q.id">
                        <new-question v-model="exam.questions[index]"></new-question>
                    </li>
                </ol>
                
            </div>
            
        </div>
        <div class="row">
            <div class="col">
                <button type="button" class="btn btn-success" @click="save">Kaydet</button>
                <button type="button" class="btn btn-primary" @click="addQuestion">Soru Ekle</button>
            </div>
            
        </div>
    </div>
</template>
<script>
    import ObjectId from '../ObjectId.js'
    import axios from 'axios'
    import newQuestion from '../components/NewQuestion.vue';
    export default {
        components: {
            newQuestion
        },
        data() {
            return {
                wiredContent: null,
                selectedContent: null,
                exam: {
                    id: ObjectId.NewId(),
                    questions: []
                }
            }
        },
        created() {
            this.getWiredContents();
        },
        methods: {
            getWiredContents() {
                axios.get('api/wired')
                    .then(x => {
                        this.wiredContent = x.data;
                    })
                    .catch(x => {
                        console.error(x);
                        this.wiredContent = [];
                    })
            },
            loadContent() {
                this.$refs.contentFrame.src = this.selectedContent.link;
                this.exam.title = this.selectedContent.title;
                this.exam.wiredUrl = this.selectedContent.link;
                //this.$refs.contentFrame.href = this.selectedContent.link;
            },
            addQuestion() {
                this.exam.questions.push({
                    id: ObjectId.NewId(),
                    choices: [
                        { id: ObjectId.NewId() },
                        { id: ObjectId.NewId() },
                        { id: ObjectId.NewId() },
                        { id: ObjectId.NewId() },
                    ]
                });
            },
            save() {
                axios.post('api/examinations', this.exam)
                    .then(x => {
                        console.log(x);
                        this.$router.push("/examinations");
                    }).
                    catch(x => {
                        console.error(x);
                        alert('Soru kaydedilemedi.');
                    })
            }
        }
    }
</script>