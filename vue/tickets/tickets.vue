<script>
    import { useVuelidate } from '@vuelidate/core';
    import { required, minLength } from '@vuelidate/validators';
    import VueDatePicker from '@vuepic/vue-datepicker';
    import dateFormat, { masks } from "dateformat";

    export default {
        components: {
            VueDatePicker
        },
        data() {
            return {
                activeTab: 'tickets-list',
                ticketsList: ticketsList,
                selectedTicket: null,
                filter: 2,

                v$: useVuelidate(),
                newTicket: {
                    creationDate: new Date(),
                    completionDate: null,
                    subject: null,
                    content: null,
                    priority: 1
                },
                subject: null,
            }
        },
        created() {

        },
        methods: {
            onTabMenuClick(tabName) {
                this.activeTab = tabName;
            },
            createTicket() {
                let that = this;
                let requestData = this.newTicket;
                requestData.creationDate = dateFormat(requestData.creationDate, "dd/mm/yyyy HH:MM:ss");
                requestData.completionDate = !!requestData.completionDate ? dateFormat(requestData.completionDate, "dd/mm/yyyy HH:MM:ss") : null;

                $.post('tickets', this.newTicket, function (response) {
                    that.newTicket = {
                        creationDate: new Date(),
                        completionDate: null,
                        subject: null,
                        content: null,
                        priority: 1
                    }

                    that.ticketsList = JSON.parse(response.data);

                    toastr.success('Ticket was succesfully created.', 'Success!');
                });
            },
            isExpiring(ticket) {
                let d1 = ticket.CreationDate;
                let d2 = ticket.CompletionDate;

                let date1 = new Date(d1).getTime();
                let date2 = new Date(d2).getTime();

                if (!ticket.IsClosed && !!d2 && date2 - date1 <= 1000 * 60 * 60) {
                    return true;
                }

                return false;
            },
            showCloseTicketModal(ticket) {
                this.selectedTicket = ticket;
                $('#close-ticket-modal').modal('show'); 
            },
            hideCloseTicketModal() {
                this.selectedTicket = null;
                $('#close-ticket-modal').modal('hide'); 
            },
            closeTicket() {
                let that = this;
                $.post('tickets/close/' + this.selectedTicket.Id, {}, function (response) {
                    that.selectedTicket.IsClosed = true;
                    that.hideCloseTicketModal();

                    toastr.success('Ticket was closed.', 'Success!');
                });
            }
        },
        computed: {
            filteredTickets() {
               let sorted = this.ticketsList.sort(function(a, b) {
                   const distantFuture = new Date(8640000000000000);
                   let d1 = !!a.CompletionDate ? new Date(a.CompletionDate) : distantFuture;
                   let d2 = !!b.CompletionDate ? new Date(b.CompletionDate) : distantFuture;
                   
                   return d1.getTime() - d2.getTime();
               });

               let that = this;
               let filtered = sorted.filter(function(ticket) {
                   if (that.filter == 1) {
                        return that.isExpiring(ticket);
                   }
                   if (that.filter == 2) {
                        return true;
                   }
                   if (that.filter == 3) {
                        return ticket.IsClosed;
                   }
               });

                return filtered;
            },
            // TODO: temporary solution, find out why vuelidate $v is undefined
            isCreateTicketBtnDisabled() {
                if (!this.newTicket.subject || this.newTicket.subject.length < 5) {
                    return true;
                }

                if (!this.newTicket.content || this.newTicket.content.length < 10) {
                    return true;
                }

                return false;
            }
        },
        validations() {
            return {
                newTicket: {
                    subject: { required, minLength: minLength(5) },
                    content: { required, minLength: minLength(10) }
                }
            }
        },
        watch: {

        }
    }
</script>

<template>
    <ul class="nav nav-tabs" id="ticket-tabs">
        <li class="nav-item">
            <a class="nav-link" :class="{ 'active': activeTab == 'tickets-list' }" href="#tickets-list" @click="onTabMenuClick('tickets-list')">Tickets list</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" :class="{ 'active': activeTab == 'create-ticket' }" href="#create-ticket" @click="onTabMenuClick('create-ticket')">Create ticket</a>
        </li>
    </ul>

    <div class="tab-content p15" id="ticket-tabs-content">
        <div class="tab-pane fade" :class="{ 'show active': activeTab == 'tickets-list' }">
            <div class="btn-group mb10" role="group" aria-label="Basic example">
                <button type="button" class="btn" :class="[filter == 1 ? 'btn-success' : 'btn-light']" @click="filter = 1">Expiring</button>
                <button type="button" class="btn" :class="[filter == 2 ? 'btn-success' : 'btn-light']" @click="filter = 2">All</button>
                <button type="button" class="btn" :class="[filter == 3 ? 'btn-success' : 'btn-light']" @click="filter = 3">Closed</button>
            </div>
            <div class="alert alert-success" v-if="!filteredTickets.length"><h6 class="mbn">List is empty</h6></div>
            <div v-else v-for="ticket in filteredTickets" class="tickets-list">
                <div class="card mb10">
                    <div class="card-body" :class="{ 'ticket-danger' : isExpiring(ticket) }">
                        <h5 class="card-title mbn pointer underline" @click="ticket.Shown = !ticket.Shown">
                            <span><i class="fa fa-plus-square-o" aria-hidden="true" v-if="!ticket.Shown"></i></span>
                            <span><i class="fa fa-minus-square-o" aria-hidden="true" v-if="ticket.Shown"></i></span>
                            <span class="ml5">{{ ticket.Subject }}</span>
                            <button type="button" v-if="!ticket.IsClosed" class="btn btn-success right" @click="showCloseTicketModal(ticket)">Close</button>
                        </h5>
                        <small class="form-text text-muted">{{ ticket.CreationDate }}</small>
                        <small v-if="!!ticket.CompletionDate" class="form-text text-muted"> - {{ ticket.CompletionDate }}</small>
                        <span class="badge badge-success ml5" v-if="ticket.Priority == 1">low</span>
                        <span class="badge badge-warning ml5" v-if="ticket.Priority == 2">medium</span>
                        <span class="badge badge-danger ml5" v-if="ticket.Priority == 3">high</span>
                        <p class="card-text collapsed" v-if="!ticket.Shown">{{ ticket.Content }}</p>
                        <p class="card-text" v-if="ticket.Shown">{{ ticket.Content }}</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane fade" :class="{ 'show active': activeTab == 'create-ticket' }">
            <form>
                <div class="form-group mb5">
                    <label>Date:</label>
                    <VueDatePicker v-model="newTicket.creationDate" :disabled="true" />
                </div>
                <div class="form-group mb5">
                    <label>Completion date:</label>
                    <VueDatePicker v-model="newTicket.completionDate" />
                </div>
                <div class="form-group mb5">
                    <label>Subject: <span class="red">*</span></label>
                    <input class="form-control" v-model="newTicket.subject" @blur="v$.newTicket.subject.$touch">
                    <div class="red" v-if="v$.newTicket.subject.$error">Minimum length is 5 characters</div>
                </div>
                <div class="form-group mb5">
                    <label>Description: <span class="red">*</span></label>
                    <textarea class="form-control rszn" rows="5" v-model="newTicket.content" @blur="v$.newTicket.content.$touch"></textarea>
                    <div class="red" v-show="v$.newTicket.content.$error">Minimum length is 10 characters</div>
                </div>
                <div class="form-group mb10">
                    <label>Priority:</label>
                    <select class="form-control" v-model="newTicket.priority">
                        <option value="1">Low</option>
                        <option value="2">Medium</option>
                        <option value="3">High</option>
                    </select>
                </div>
                <button type="button" class="btn btn-success right" @click="createTicket" :disabled="isCreateTicketBtnDisabled">Create</button>
            </form>
        </div>
    </div>


    <div class="modal" tabindex="-1" role="dialog" id="close-ticket-modal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Close ticket</h5>
                </div>
                <div class="modal-body">
                    <p>Are you sure you want to close the ticket?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" @click="closeTicket">Yes</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" @click="hideCloseTicketModal">No</button>
                </div>
            </div>
        </div>
    </div>
</template>