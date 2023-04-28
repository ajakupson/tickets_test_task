<script>
    export default {
        data() {
            return {
                activeTab: 'create-ticket'
            }
        },
        methods: {
            onTabMenuClick(tabName) {
                this.activeTab = tabName;
            }
        },
    }
</script>

<template>
    <ul class="nav nav-tabs" id="ticket-tabs">
        <li class="nav-item">
            <a class="nav-link" :class="{ 'active': activeTab == 'create-ticket' }" href="#create-ticket" @click="onTabMenuClick('create-ticket')">Create ticket</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" :class="{ 'active': activeTab == 'tickets-list' }" href="#tickets-list" @click="onTabMenuClick('tickets-list')">Ticket list</a>
        </li>
    </ul>

    <div class="tab-content p15" id="ticket-tabs-content">
        <div class="tab-pane fade" :class="{ 'show active': activeTab == 'create-ticket' }">Form</div>
        <div class="tab-pane fade" :class="{ 'show active': activeTab == 'tickets-list' }">List</div>
    </div>
</template>