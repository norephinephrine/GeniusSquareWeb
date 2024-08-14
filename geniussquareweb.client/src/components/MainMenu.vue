<template>
    <div>
        <h1 style="color: darkgreen;"> Join existing game</h1>
        <ul>
            <li v-for="(gameId, index) in gameInstanceList"> 
                <button 
                    class="link"
                    @click="joinGameRoom(gameId)">
                    {{gameId}}
                </button>
            </li>
        </ul>
        <button 
            @click="createGameRoom">
            Create a new game
        </button>
    </div>
</template>

<script lang="ts">
    import * as signalR from "@microsoft/signalr";
    import { defineComponent, type PropType } from 'vue';
    import type { GameData } from "./GameLogic/GameTypes";

    export default defineComponent({
        props: {
            gameInstanceList : Array<string>,
            signalRConnection :
            {
                type: Object as PropType<signalR.HubConnection>,
                required: true,

            },
        },
        methods: {
                async createGameRoom()
                {
                    if (this.signalRConnection == undefined)
                    {
                        throw new Error('SignalR connection should not be null');
                    }

                    let gameRecord:GameData = await this.signalRConnection.invoke("CreateGameAsync")
                    .catch(ex =>
                    {
                        console.log(ex);
                    });

                    if(gameRecord == null)
                    {
                        alert("Failed to create game");
                        return;
                    }
                    
                    this.$emit("createGame", gameRecord);

                },
                async joinGameRoom(gameId: string)
                {

                    let gameRecord:GameData = await this.signalRConnection.invoke("JoinGameAsync", gameId)
                    .catch(ex =>
                    {
                        console.log(ex);
                    });

                    if(gameRecord == null)
                    {
                        alert("Game is full. Try another one.");
                        return;
                    }
                                   
                    this.$emit("joinGame", gameRecord);
                }
            },
        },
    );
</script>

<style scoped>

ul{height:400px; width:400px;}
ul{overflow:hidden; overflow-y:scroll;}

button.link { 
    background:none;
    border:none;
    color: grey;
}

button.link:hover{
    color: red;
}
</style>