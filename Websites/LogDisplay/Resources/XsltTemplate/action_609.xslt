<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="age_customerupduserinfo">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:custID}</td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">
                            
                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:custid}</td>
                        <td class="colW2">
                        
                            <xsl:value-of select="@custid" />
                        </td>
                        
                    </xsl:otherwise>
                </xsl:choose>-->
                <td class="colW1">{lbl:subAccName}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1">{lbl:firstName}</td>
                <td class="colW2">
                    <xsl:value-of select="@firstname" />
                </td>
                <td class="colW1">{lbl:lastName}</td>
                <td class="colW2">
                    <xsl:value-of select="@lastname" />
                </td>

            </tr>
            <tr>
                <td class="colW1">{lbl:email}</td>
                <td class="colW2">
                    <xsl:value-of select="@email" />
                </td>
                <td class="colW1">{lbl:phone}</td>
                <td class="colW2">
                    <xsl:value-of select="@phone" />
                </td>
                <td class="colW1">{lbl:mobilePhone}</td>
                <td class="colW2">
                    <xsl:value-of select="@mobilephone" />
                </td>

            </tr>
            <tr>
                <td class="colW1">{lbl:fax}</td>
                <td class="colW2">
                    <xsl:value-of select="@fax" />
                </td>
                <td class="colW1">{lbl:ip}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>


                <!--<td class="colW1">{lbl:userID}</td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->
                <td class="colW1">{lbl:language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>

            </tr>
            <tr>
                <td class="colW1">{lbl:errCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">{lbl:errMsg}</td>
                <td class="colW2" >
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colW1"></td>
                <td class="colW2" >

                </td>


            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>
