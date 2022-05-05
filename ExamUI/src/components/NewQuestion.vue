<template>
    <fieldset>
        <div class="row">
            <div class="col">
                Soru:
                <textarea required v-model="value.content" class="form-control" />
            </div>
        </div>
        <div class="row">
            <ol style="list-style-type: lower-alpha">
                <li class="float-start" style="width:44%; margin-right:3%; margin-left:3%;" v-for="c in value.choices" :key="c.id">
                    <div class="input-group mb-3">
                        <input type="text" required class="form-control" v-model="c.text">
                        <button class="btn btn-outline-secondary" type="button" @click="removeChoice(c)">Sil</button>
                    </div>
                </li>
            </ol>
        </div>
        <div class="row">
            <div class="col">
                Doğru şık: 
                <select v-model="value.correctChoiceId" required>
                    <option :value="c.id" v-for="c in value.choices" :key="c.id">{{c.text}}</option>
                </select>
            </div>
            <div class="col">
                <button class="btn btn-sm btn-outline-secondary float-end" type="button" @click="addChoice">Şık ekle</button>
            </div>
        </div>
        <hr />
    </fieldset>
</template>
<script>
    import ObjectId from '../ObjectId.js'

    export default {
        model: {
            prop: 'value',
            event: 'change'
        },
        props: {
            value: Object
        },
        methods: {
            setValue(val) {
                this.$emit('change', val?.id)
            },
            addChoice() {
                this.value.choices.push({
                    id: ObjectId.NewId()
                });
            },
            removeChoice(c) {
                this.value.choices = this.value.choices.filter(x => x != c);
            }
        }
    }
</script>